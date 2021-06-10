//SPDX-License-Identifier: Unlicensed

pragma solidity ^0.6.12;

import {IBEP20, SafeMath} from "./MoonKat.sol";

contract PreSale {
    using SafeMath for uint256;

    string public termsAndConditions =
        "By interacting with this contract, I confirm I am not a US citizen. I agree to be bound by the terms found at [termsAndConditionUrl]";

    IBEP20 public tokenOnSale;

    uint256 public saleStart;
    uint256 public saleDuration;

    uint256 public saleEndTimestamp;

    address payable moneyTransferTo;

    uint256 public minBNBAmount = 2 * 10**17; // 0.2 ether
    uint256 public maxBNBAmount = 3 ether;

    uint256 public oneTokenPriceInBNB = 1333333333000;

    uint256 public reservedTokens;

    mapping(address => uint256) public reservedTokensToAddress;

    address[] public buyers;

    address[] public whitelist;

    // first payment - is when user gets 40% of boughth tokens
    bool public secondPaymentSucceed;
    bool public thirdPaymentSucceed;

    uint256 public monthDuration = 30 days;

    modifier onlyInActive() {
        require(!_isSalePeriodEnd(), "Sale is not active");
        _;
    }
    modifier onlyAfterEnd() {
        require(saleEndTimestamp < block.timestamp, "Is not ended yet");
        _;
    }

    modifier onlyAfterStart() {
        require(saleStart < block.timestamp, "Is not started yet");
        _;
    }

    constructor(
        // address, to where all BNB from buy method will be forwarded.
        // Also, all remaining tokens, that were didn`t sold
        // Will be transfered to this address
        address payable _moneyTransferTo,
        uint256 _saleStart,
        uint256 _saleDuration,
        address _tokenOnSale,
        address[] memory _whitelist
    ) public {
        require(_saleStart != 0, "Sale start should be greater than 0");
        require(_saleDuration != 0, "Sale duration should be greater than 0");
        require(
            _tokenOnSale != address(0),
            "You should provide valid token address"
        );
        require(
            _moneyTransferTo != address(0),
            "You should provide valid forwarded address"
        );

        tokenOnSale = IBEP20(_tokenOnSale);
        saleStart = _saleStart;
        saleDuration = _saleDuration;
        moneyTransferTo = _moneyTransferTo;
        saleEndTimestamp = _saleStart + _saleDuration;
        whitelist = _whitelist;
    }

    function buy() public payable onlyInActive {
        require(msg.value > 0, "Value > 0 must be send with transaction");
        require(
            msg.value >= minBNBAmount,
            "Value must be bigger then minBNBAmount"
        );
        require(
            msg.value <= maxBNBAmount,
            "Value must be less then maxBNBAmount"
        );
        if (whitelist.length != 0)
            require(
                _isContainsInAddressArray(msg.sender, whitelist),
                "You not included into the whitelist"
            );

        uint256 tokensAmount = _calculateTokenAmount(msg.value);

        require(
            tokenOnSale.balanceOf(address(this)) - reservedTokens >=
                tokensAmount,
            "PreSale inssufisient token balance"
        );

        uint256 tokensToPayNow = tokensAmount.mul(4).div(10); // 40% of bought tokens

        uint256 tokensToReserve = tokensAmount - tokensToPayNow;

        tokenOnSale.transfer(msg.sender, tokensToPayNow);

        _sendValue(moneyTransferTo, msg.value);

        if (!_isContainsInAddressArray(msg.sender, buyers))
            buyers.push(msg.sender);

        reservedTokensToAddress[msg.sender] += tokensToReserve;
        reservedTokens += tokensToReserve;
    }

    function withdrawTokens() public onlyAfterEnd {
        if (!secondPaymentSucceed) {
            require(
                saleEndTimestamp + monthDuration < block.timestamp,
                "It`s to soon to make 2/3 payment"
            );

            for (uint256 i; i < buyers.length; i++)
                _withdrawBuyerReservedTokens(
                    buyers[i],
                    reservedTokensToAddress[buyers[i]] / 2 // 30% in first month
                );

            secondPaymentSucceed = true;
        }

        if (!thirdPaymentSucceed) {
            require(
                saleEndTimestamp + 2 * monthDuration < block.timestamp,
                "It`s to soon to make 3/3 payment"
            );

            for (uint256 i; i < buyers.length; i++)
                _withdrawBuyerReservedTokens(
                    buyers[i],
                    reservedTokensToAddress[buyers[i]] // 30% (rest tokens) in second month
                );

            thirdPaymentSucceed = true;
        }
    }

    function withdrawRemainderTokens() public onlyAfterEnd {
        tokenOnSale.transfer(
            moneyTransferTo,
            tokenOnSale.balanceOf(address(this))
        );
    }

    function getCurentTime() public view returns (uint256) {
        return block.timestamp;
    }

    function getTokenSupply() public view returns (uint256) {
        return tokenOnSale.balanceOf(address(this)) - reservedTokens;
    }

    function calculateTokenAmount(uint256 _bnbAmount)
        public
        view
        returns (uint256)
    {
        return _calculateTokenAmount(_bnbAmount);
    }

    function _withdrawBuyerReservedTokens(
        address _buyer,
        uint256 _amountToWithdraw
    ) private {
        tokenOnSale.transfer(_buyer, _amountToWithdraw);

        reservedTokens -= _amountToWithdraw;
        reservedTokensToAddress[_buyer] -= _amountToWithdraw;
    }

    function _calculateTokenAmount(uint256 _bnbAmount)
        private
        view
        returns (uint256)
    {
        return _bnbAmount.div(oneTokenPriceInBNB);
    }

    function _isContainsInAddressArray(address addr, address[] memory array)
        private
        pure
        returns (bool)
    {
        for (uint256 i; i < array.length; i++)
            if (array[i] == addr) return true;
        return false;
    }

    function _isSalePeriodEnd() private view returns (bool) {
        require(saleStart < block.timestamp, "Sale didn`t started yet");
        return saleEndTimestamp < block.timestamp;
    }

    function _sendValue(address payable to, uint256 amount) private {
        require(address(this).balance >= amount, "Insufficient balance");
        (bool success, ) = to.call{value: amount}("");
        require(success, "Unable to send value, recipient may have reverted");
    }
}
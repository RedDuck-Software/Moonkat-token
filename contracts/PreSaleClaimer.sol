//SPDX-License-Identifier: Unlicensed
pragma solidity ^0.6.12;

import "./MoonKat.sol";

contract PreSaleClaimer is Ownable{ 
    using SafeMath for uint256;

    struct ClaimerInfo {
        uint256 totalTokensAmount;
        uint256 periodPaymentAmount; 
        uint256 paymentsMade;
        bool isValue;
    }

    uint256 public constant unFreezePeriod = 1 days;
    
    uint256 public constant unFreezePercentage = 2; // 2%

    uint256 public percentageBasicPoints = 10 ** 9;

    uint256 public immutable claimAvailableFrom; 

    IBEP20 public immutable mkatToken;

    mapping (address => ClaimerInfo) public tokenClaimers;

    address[] private claimers;

    constructor(uint256 _claimAvailableFrom, address _mkatAddress) public { 
        require(_claimAvailableFrom != 0, "Invalid value: claimAvalableFrom must be > 0");
        require(_mkatAddress != address(0), "Invalid value: mkatAddress");

        mkatToken = IBEP20(_mkatAddress);
        claimAvailableFrom = _claimAvailableFrom;
    }
    

    function claimTokens() public { 
        ClaimerInfo storage senderInfo = tokenClaimers[msg.sender];

        require(claimAvailableFrom < block.timestamp, "Claiming is not started yet");
        require(senderInfo.isValue, "Address is not in the claim list");
        require(senderInfo.paymentsMade.mul(senderInfo.periodPaymentAmount) < senderInfo.totalTokensAmount,
            "All tokens is already withdrawed");

        uint256 passedPeriodPaymentsCount = _calculatePassedPeriodPaymentsCount();

        require( passedPeriodPaymentsCount > senderInfo.paymentsMade, 
          "No unfreezed tokens available for now");
        
        uint256 tokensToSend = passedPeriodPaymentsCount.sub(senderInfo.paymentsMade).mul(senderInfo.periodPaymentAmount);

        senderInfo.paymentsMade = passedPeriodPaymentsCount;

        mkatToken.transfer(msg.sender, tokensToSend);

        tokenClaimers[msg.sender] = senderInfo;
    }

    function calculateTokenAmountNeededForClaimers() public onlyOwner view returns (uint256) {
        uint256 amount;

        for(uint i; i <claimers.lenght;i++ ) 
            amount += tokenClaimers[claimers[i]].totalTokensAmount;

        return amount;
    }

    function _addTokenClaimer(address _claimerAddress, uint256 _tokensAmount) private { 
        require(!tokenClaimers[_claimerAddress].isValue, "Address is already in the calim list");

        tokenClaimers[_claimerAddress] = ClaimerInfo(_tokensAmount, _calculatePeriodPaymentAmount(_tokensAmount, percentageBasicPoints), 0, true);
        claimers.push(_claimerAddress);
    }


    function _calculatePeriodPaymentAmount(uint256 _totalAmount, uint256 _basicPoints) private pure returns (uint256){ 
        return _totalAmount.mul(unFreezePercentage.mul(_basicPoints)).div(uint256(100).mul(_basicPoints));
    }

    function _calculateUnFreezeAmount(address _claimerAddress) private view { 

    }

    function _calculatePassedPeriodPaymentsCount() private view returns (uint256){ 
        require(claimAvailableFrom < block.timestamp);

        return claimAvailableFrom.sub(block.timestamp).div(unFreezePeriod);
    }


    function _hardcodeAddresses() private {

    }
}
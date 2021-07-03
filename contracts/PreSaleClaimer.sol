//SPDX-License-Identifier: Unlicensed
pragma solidity ^0.6.12;

import "./MoonKat.sol";

contract PreSaleClaimer is Ownable{ 
    using SafeMath for uint256;

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

    }


    function addClaimerAddress(address _claimerAddress, uint256 _tokensAmount) public onlyOwner{ 
        _addTokenClaimer(_claimerAddress, _tokensAmount);
    }


    function _addTokenClaimer(address _claimerAddress, uint256 _tokensAmount) private { 
        require(!tokenClaimers[_claimerAddress].isValue, "Address is already in the list");

        tokenClaimers[_claimerAddress] = ClaimerInfo(_tokensAmount, _calculatePeriodPaymentAmount(_tokensAmount, percentageBasicPoints), 0, true);
        claimers.push(_claimerAddress);
    }


    function _calculatePeriodPaymentAmount(uint256 _totalAmount, uint256 _basicPoints) private pure returns (uint256){ 
        return _totalAmount.mul(unFreezePercentage.mul(_basicPoints)).div(100 * _basicPoints);
    }

    function _calculateUnFreezeAmount(address _claimerAddress) private view { 

    }

    function _calculatePassedPeriodPaymentsCount() private view returns (uint256){ 
        require(claimAvailableFrom < block.timestamp);

        return claimAvailableFrom.sub(block.timestamp).div(1 days);
    }


    function _hardcodeAddresses() private {

    }


    struct ClaimerInfo {
        uint256 totalTokensAmount;
        uint256 periodPaymentAmount; 
        uint256 paymentsMade;
        bool isValue;
    }
}
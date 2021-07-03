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

    uint256 public constant maxPayments = 100 / unFreezePercentage; // 50 payments

    uint256 public immutable claimAvailableFrom; 

    IBEP20 public immutable mkatToken;

    mapping (address => ClaimerInfo) public tokenClaimInfoFor;

    address[] private claimersList;

    constructor(uint256 _claimAvailableFrom, address _mkatAddress) public { 
        require(_claimAvailableFrom != 0, "Invalid value: claimAvalableFrom must be > 0");
        require(_mkatAddress != address(0), "Invalid value: mkatAddress");

        mkatToken = IBEP20(_mkatAddress);
        claimAvailableFrom = _claimAvailableFrom;

        _hardcodeAddresses();
    }
    

    function claimTokens() public { 
        ClaimerInfo storage senderInfo = tokenClaimInfoFor[msg.sender];

        require(claimAvailableFrom < block.timestamp, "Claiming is not started yet");
        require(senderInfo.isValue, "Address is not in the claim list");
        require(senderInfo.paymentsMade.mul(senderInfo.periodPaymentAmount) < senderInfo.totalTokensAmount,
            "All tokens is already withdrawed");

        uint256 passedPeriodPaymentsCount = _calculatePassedPeriodPaymentsCount();

        if(passedPeriodPaymentsCount > maxPayments) 
            passedPeriodPaymentsCount = maxPayments;

        require( passedPeriodPaymentsCount > senderInfo.paymentsMade, 
          "No unfreezed tokens available for now");
        
        uint256 tokensToSend = passedPeriodPaymentsCount.sub(senderInfo.paymentsMade).mul(senderInfo.periodPaymentAmount);

        senderInfo.paymentsMade = passedPeriodPaymentsCount;


        mkatToken.transfer(msg.sender, tokensToSend);

        tokenClaimInfoFor[msg.sender] = senderInfo;
    }

    function calculateTokenAmountNeededForClaimers() public onlyOwner view returns (uint256) {
        uint256 amount;

        for(uint i; i <claimersList.length;i++ ) 
            amount += tokenClaimInfoFor[claimersList[i]].totalTokensAmount;

        return amount;
    }

    function _addTokenClaimer(address _claimerAddress, uint256 _tokensAmount) private { 
        require(!tokenClaimInfoFor[_claimerAddress].isValue, "Address is already in the calim list");

        tokenClaimInfoFor[_claimerAddress] = ClaimerInfo(_tokensAmount, _calculatePeriodPaymentAmount(_tokensAmount), 0, true);
        claimersList.push(_claimerAddress);
    }

    function _calculatePeriodPaymentAmount(uint256 _totalAmount) private pure returns (uint256){ 
        return _totalAmount.mul(unFreezePercentage).div(uint256(100));
    }

    function _calculatePassedPeriodPaymentsCount() private view returns (uint256){ 
        require(claimAvailableFrom < block.timestamp);

        return claimAvailableFrom.sub(block.timestamp).div(unFreezePeriod);
    }


    function _hardcodeAddresses() private {
        _addTokenClaimer(address(0x03CE7ad9E91Ca95576B90B00B780328677c8DC5F), 450000000000000);
        _addTokenClaimer(address(0x05Fb9594BbF49979D5Ca5c95a913BF4995F4C096), 450000000000000);
        _addTokenClaimer(address(0x06e5a9feb9e33BCae23a2f53E70A513fba96bc77), 500000000000000);
        _addTokenClaimer(address(0x0bA2f9edB9734F5c6715F86079649f3814C5Fd98), 2500000000000000);
        _addTokenClaimer(address(0x0E08d3048c5435d30eF83c55915227e3ca4Aa3BA), 1050000000000000);
        _addTokenClaimer(address(0x0Ec8BC018C50502254A1f257471698212bC54cC7), 225000000000000);
        _addTokenClaimer(address(0x14318237a1f45792533eB5A34925245f17a4F660), 2100000000000000);
        _addTokenClaimer(address(0x19689D6f4AD16bdec73f9648326A38C27BfBe961), 450000000000000);
        _addTokenClaimer(address(0x20e608FD691F3797aFA3fC879F5475A9DBd41818), 1000000000000000);
        _addTokenClaimer(address(0x2a2513585F2831B9E99A9f81812B12b8A1E2C949), 112500000000000);
        _addTokenClaimer(address(0x2F4dA44395E4a044F271936087c0D7C369af4E27), 250000000000000);
        _addTokenClaimer(address(0x32Ea2BcEe4eB91bA1a1DA65901217341bCdFbCDF), 450000000000000);
        _addTokenClaimer(address(0x39bd6d38c8655A7A7A8a1F5A7f09893db7DA0716), 1000000000000000);
        _addTokenClaimer(address(0x40F5344A84b9Eb5071AcDe3DbAC14AbDa592d482), 252083750000000);
        _addTokenClaimer(address(0x46D5190B9697E6462fFB0beDfACF71b627d338eC), 450000000000000);
        _addTokenClaimer(address(0x495e2cb0bAccd12a6245a903BacD53d81c735c3D), 450000000000000);
        _addTokenClaimer(address(0x4EA90528906889D1c76d822BA2171945f792B175), 700000000000000);
        _addTokenClaimer(address(0x4fE52b35Cc196Ac681f2fF2b6455A50c3Ad96929), 750905500000000);
        _addTokenClaimer(address(0x50bD80eE272Ae00C39365fBc2385F2B4029DfC28), 500000000000000);
        _addTokenClaimer(address(0x52704f6700Dc5D597176D83250FbD8012704e394), 427850000000000);
        _addTokenClaimer(address(0x541Ea321242C96F75f2fAA19166794e247b57527), 450000000000000);
        _addTokenClaimer(address(0x5573A64B7676b43Cf00A47566Bb2e0519fEb98d4), 450000000000000);
        _addTokenClaimer(address(0x5e84DEC6cD3CC3928abF47f45Dd66dE3c3e46295), 287500000000000);
        _addTokenClaimer(address(0x675169C8283b4346d9cB31a64De343E38a13B776), 500000000000000);
        _addTokenClaimer(address(0x705422D67ecbb2A8C8CFfFF822156638727B0972), 450000000000000);
        _addTokenClaimer(address(0x72372152695e67A197F65f9E045C1cA59B640F7E), 2500000000000000);
        _addTokenClaimer(address(0x7F32378806e6bd9a8fF893B240dD5bD0646BE931), 2025000000000000);
        _addTokenClaimer(address(0x7f4661627705074934f26305f66D7A2F833578AA), 450000000000000);
        _addTokenClaimer(address(0x80E8Ff6B4fE97DA01118dE914AD600C1eF87177A), 450000000000000);
        _addTokenClaimer(address(0x8208843552f7BB224C0e121E4806dF5977dAbaa5), 1000000000000000);
        _addTokenClaimer(address(0x859069549D0279efE4e94Fe2508bf4bAa996Cb5A), 112500000000000);
        _addTokenClaimer(address(0x87A0dd0652147FBCBa18b1407153f0aE767baCc3), 450000000000000);
        _addTokenClaimer(address(0x8aa2091f4D3Af46130715aa33Acadd37230cc5B6), 450000000000000);
        _addTokenClaimer(address(0x8cfB5d4e4204162439b3714a5eC7a3112575148F), 2338350000000000);
        _addTokenClaimer(address(0x8d2BF841c75472782c3269ca3156A251b85dcA1A), 450000000000000);
        _addTokenClaimer(address(0x90c42352f7EDc8b3F0b6782F2D22e004BD3964F7), 450000000000000);
        _addTokenClaimer(address(0x9465dd499F3667B969Fc8F5d8f5D33FA98Ca6643), 450000000000000);
        _addTokenClaimer(address(0x97CbcDCa693BD6f38f49Be405b04344D835052c3), 1000000000000000);
        _addTokenClaimer(address(0x9C6d4c10df976205DE101f8039184B2a8191f0a7), 450000000000000);
        _addTokenClaimer(address(0x9eD5400f2EDB65a357C11bd947Ca8127a163cd10), 1500000000000000);
        _addTokenClaimer(address(0xa52846098b5834A4B1f4BbF4D2787Fa20d9E0F31), 450000000000000);
        _addTokenClaimer(address(0xA9CC6E754a8e50aB71653f10D2E7D03CcB778fA6), 450000000000000);
        _addTokenClaimer(address(0xaD79E0E4a7F83FbDFbA5B9F3b18Eb69988f8f091), 450000000000000);
        _addTokenClaimer(address(0xAe81605C20efd578442123AAf00C9a35bBD792cD), 2000000000000000);
        _addTokenClaimer(address(0xb7C1887e6C9ABDcac94CD35635181221acC918Ce), 1000000000000000);
        _addTokenClaimer(address(0xBf02976e904908E3421a1a2303B7a2B13ffC7Db3), 1000000000000000);
        _addTokenClaimer(address(0xc1fd10042bDF39977D03EA072C9bbd23f5AD46C4), 2500000000000000);
        _addTokenClaimer(address(0xCdA9d2334A2473373894c5e4fA4a9295eED0294E), 225000000000000);
        _addTokenClaimer(address(0xd338Ad481658CAB28Ce0fe54c457F97D78b77227), 300000000000000);
        _addTokenClaimer(address(0xdD81016a2226f1dd14BBD765D2B153FacF0C0aB5), 450000000000000);
        _addTokenClaimer(address(0xDf0b112E1194eF2Ae50E38F4f3ea498DFFE1Ded7), 255000000000000);
        _addTokenClaimer(address(0xE1ee78b8360Eb51f3E4c6021ba6FBde3F7dC38D9), 450000000000000);
        _addTokenClaimer(address(0xe243600aD7FD19d828B8d13F5206242cecEE3D24), 250000000000000);
        _addTokenClaimer(address(0xe94B8bD697c123d3AAdDcc69FAADc169Ae7D6A75), 450000000000000);
        _addTokenClaimer(address(0xeeB16F4Adc89646A560b28a93c5414c4d3e0bEac), 450000000000000);
        _addTokenClaimer(address(0xFD0F6cf0A329b3f2d3022D6e9C6c2D48EbDFF011), 1000000000000000);
        _addTokenClaimer(address(0xfed589bb82D3cAE51dfC5Ffed4a9A50627df300C), 2025000000000000);
    }
}
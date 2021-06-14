# HOW TO DEPLOY CONTRACTS

1. Run ```npm i``` to install all dependencies
2. Install truffle globally ```npm install -g truffle```
3. Rename ```secrets.example.json``` to ```secrets.json``` and replace ```mnemonic``` value with real one
4. Compile you contracts using ```truffle compile```
5. Change ```SwapRouterAddress``` in ```1_migration.js``` file (migration folder)
6. Deploy your contracts
    ```truffle migrate --network [network] --reset```

Possible \[network\] values you can find in ```truffle-config.js``` file

# HOW TO RUN TESTS

For running test you need to have [.NET 5 SDK](https://dotnet.microsoft.com/download/dotnet/5.0) installed 

1. Run hardhat node ```npx hardhat node --fork https://eth-mainnet.alchemyapi.io/v2/{ALCHEMY_API_KEY}``` 
2. Go to tests/Test folder
3. Rename ```secrets.example.json``` to ```secrets.json```
4. Replace ```alchemyApiKey``` value with your real Alchemy API key

Now you can run your tests

Run CMD in tests/Test folder and execute ```dotnet test``` command
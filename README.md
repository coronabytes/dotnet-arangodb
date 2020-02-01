# Disclaimer
- This is a minimalistic .NET driver for ArangoDB (3.5+) with adapters to Serilog and DevExtreme
- The key difference to any other available driver is the ability to switch databases on a per request basis, which allows for easy database per tenant deployments
- Id, Key, From, To properties will always be translated to their respective arango form (_id, _key, _from, _to), which allows to construct updates from anonymous types
- No documentation or samples yet

# Troubleshooting running on Linux

* You cannot (currently) use project references for native libraries, you must pass via a NuGet repository

## References
* https://dotnet.github.io/docs/core-concepts/libraries/libraries-with-cli.html#how-to-use-native-dependencies
* https://github.com/dotnet/cli/issues/710

## Troubleshooting

```
LD_DEBUG=libs COREHOST_TRACE=1 ~/dotnet/dotnet run 2> out.txt
```

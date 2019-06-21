#!/usr/bin/env bash
dotnet restore
dotnet publish -c Release -r linux-x64
# mv ./bin/Release/netcoreapp3.0/linux-x64/native/zplify ./bin/Release/netcoreapp3.0/linux-x64/native/zplify
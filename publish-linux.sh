#!/usr/bin/env bash
dotnet restore
dotnet publish Zplify/Zplify.csproj -c Release -r linux-x64 -p:PublishSingleFile=true -o zplify
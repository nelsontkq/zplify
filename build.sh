#!/usr/bin/env bash
cd zplify
dotnet restore
dotnet publish -c Release -r linux-x64
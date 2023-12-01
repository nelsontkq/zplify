#!/usr/bin/env bash
# Detect the platform (similar to $OSTYPE)
OS="`uname`"
if [[ "$1" ]]; then
    type="$1"
else
    case $OS in
      'Linux') type='linux-x64' ;;
      'WindowsNT') type='win-x64' ;;
      'Darwin') type='osx-x64' ;;
      *) ;;
    esac
fi

dotnet publish Zplify/Zplify.csproj \
    -c Release \
    -r $type \
    -p:PublishReadyToRun=true -p:PublishSingleFile=true -p:PublishTrimmed=true \
    -o publish \
    --self-contained true
#! /bin/sh

# Run this in Ubuntu 14.04, Ubuntu 16.04, macOS, or Windows 10 with dotnet utilities installed
# Install dotnet utilities from: https://www.microsoft.com/net/core

# Special notes for Windows: run bash scripts with Git Bash

mkdir ../out

dotnet restore **/project.json
dotnet build **/project.json

for proj in compiler corelib tester tests
do
  cp -R $proj/bin/Debug/netstandard1.6.1/* ../out
done
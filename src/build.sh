#! /bin/bash

# Run this in Ubuntu, Windows, or macOS with dotnet utilities installed
# Install dotnet utilities from: https://www.microsoft.com/net/core

# Special notes for Windows: run bash scripts with Git Bash

if [ ! -e ../out ]
then
  mkdir ../out
fi

dotnet restore **/project.json
dotnet build **/project.json

for proj in compiler corelib tester tests
do
  cp -R $proj/bin/Debug/netstandard1.6.1/* ../out
done

cp -R cc/* ../out

#! /bin/bash

# Run this in Ubuntu, Windows, or macOS with dotnet utilities installed
# Install dotnet utilities from: https://www.microsoft.com/net/core

# Special notes for Windows: run bash scripts with Git Bash

uname=$(uname)

if [[ $uname =~ ^Linux* ]]
then
  runtime="ubuntu.16.04-x64"
elif [[ $uname =~ ^MINGW* ]]
then
  runtime="win10-x64"
elif [[ $uname =~ ^Darwin* ]]
then
  runtime="osx.10.12-x64"
else
  echo "Unable to determine the current platform"
  exit 1
fi

if [ ! -e ../out ]
then
  mkdir ../out
fi

dotnet restore **/project.json
dotnet build **/project.json -r $runtime

for proj in compiler corelib tester tests
do
  cp -R $proj/bin/Debug/netstandard1.6.1/* ../out
done

cp -R cc/* ../out

#! /bin/sh

# Run this in Ubuntu, Windows, or macOS with dotnet utilities installed
# Install dotnet utilities from: https://www.microsoft.com/net/core

# Special notes for Windows:
# 1. Run bash scripts with Git Bash
# 2. Include the MSVC compiler directory into PATH. E.g.: PATH=$PATH:/c/Program\ Files\ \(x86\)/Microsoft\ Visual\ Studio\ 14.0/VC/bin/amd64

uname=$(uname)

if [[ $uname =~ ^Linux* ]]
then
  plat="ubuntu.16.04-x64"
  cc="gcc"
elif [[ $uname =~ ^MINGW* ]]
then
  plat="win10-x64"
  cc="msvc"
elif [[ $uname =~ ^Mac* ]]
then
  plat="osx.10.12-x64"
  cc="llvm"
else
  echo "Unable to determine the current platform"
  exit 1
fi

echo "Platform=$plat CC=$cc.json"

if [ $1 ]
then
  tclist=$(ls tests/$1.cs)
else
  tclist=$(ls tests/*.cs)
fi

if [ ! -e ../out/$plat/ut ]
then
  mkdir ../out/$plat/ut
fi

for tc in $tclist
do
  echo ""
  echo "Test case $tc"
  
  ../out/$plat/tester $tc ../out/$plat/ut ../out/$cc.json
done

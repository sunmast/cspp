#pragma once

#include <cspp.h>

namespace System { 
class Environment {

public:

	static _<sys::string> NewLine;
};
}

namespace System { 
class IDisposable {

public:
	virtual ~IDisposable() {}

	virtual void Dispose() = 0;
};
}

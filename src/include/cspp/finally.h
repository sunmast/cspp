#pragma once

#include "../cspp.h"

// This class will be used like a keyword, so no name space here.
class finally
{
    std::function<void(void)> func;

public:
    finally(const std::function<void(void)>& func)
        : func(func)
    {
    }

    ~finally()
    {
        func();
    }
};

#pragma once

#include "../cspp.h"

/*
Notes:
It's generally not recommended to extend STL class std::basic_string because it has no virtual destructor.
Don't add member fields to this class to keep it fully compatible with std::basic_string
*/

namespace sys
{
template <typename T> class base_string : public std::basic_string<T>
{
public:

    base_string(const T* s)
        : std::basic_string<T>(s)
    {
    }

    base_string(const std::basic_string<T>& copy)
        : std::basic_string<T>(copy)
    {
    }

    static base_string<T> Concat(const base_string<T>& s1, const base_string<T>& s2)
    {
        return s1 + s2;
    }

    operator const T*()
    {
        return this->c_str();
    }
};
}

// mutable strings
typedef sys::base_string<char> string;
typedef sys::base_string<wchar_t> wstring;

// constant strings
typedef const char* cstring;
typedef const wchar_t* wcstring;

// mutable or constant stringstream
// These should only be used as a parameter type to allow passing different types of strings to a function
typedef fmt::CStringRef xstring;
typedef fmt::WCStringRef wxstring;

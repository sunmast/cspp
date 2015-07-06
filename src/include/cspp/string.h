#pragma once

#include "../cspp.h"

/*
Notes:
It's generally not recommended to extend STL class std::basic_string because it has no virtual destructor.
But to support some C# syntaxes we must extend it.
To prevent potential issues:
1. Don't put this class in a instance of std::basic_string<T>*
2. Don't add member fields to this class
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

    static _<base_string<T> > Concat(const _<base_string<T> >& s1, const _<base_string<T> >& s2)
    {
        return s1 + s2;
    }

    operator const T*()
    {
        return this->c_str();
    }
};
}

typedef sys::base_string<char> string;
typedef sys::base_string<wchar_t> wstring;

class _string : public _<string>
{
public:

    _string(const char* str) : _<string>(new_<string>(str))
    {
    }
};

class _wstring : public _<wstring>
{
public:

    _wstring(const wchar_t* wstr) : _<wstring>(new_<wstring>(wstr))
    {
    }
};

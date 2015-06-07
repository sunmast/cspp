#pragma once

#include "../cspp.h"

namespace sys
{
template <typename T> struct sptr
{
private:

    inline uint32_t& rc()
    {
        return *(uint32_t*)(ptr + 1);
    }

public:

    T* ptr; // the size of sptr is same as a pointer

    sptr()
        : ptr(nullptr)
    {
    } // some containers always need a default constructor

    sptr(T* ptr)
        : ptr(ptr)
    {
        if(!ptr)
            return;
        rc() = 1;
    }

    sptr(const sptr<T>& copy)
        : ptr(copy.ptr)
    {
        if(!ptr)
            return;

        rc()++;
    }

    ~sptr()
    {
        if(!ptr)
            return;

        uint32_t& c = rc();
        c--;

        if(c == 0)
        {
            delete ptr;
            ptr = nullptr;
        }
    }

    // Operators pass-through. Refer to http://en.wikipedia.org/wiki/Operators_in_C_and_C%2B%2B
    // The restriction here is return type only support bool or sptr<T>

    // Member and pointer operators

    T* operator->()
    {
        return ptr;
    }

    T operator&()
    {
        return *ptr;
    }

    T& operator*()
    {
        return *ptr;
    }

//    template <typename R, typename N> R& operator[](const N& index)
//    {
//        return (*ptr)[index];
//    }

//    template <typename R, typename N> R& IndexOf(const N& index)
//    {
//        return (*p)[index];
//    }

    template <typename R> inline R& IndexOf(const size_t& index)
    {
        return (*ptr)[index];
    }

    // Arithmetic operators

    sptr<T>& operator=(const sptr<T>& operand)
    {
        ptr = operand.ptr;
        if(!ptr)
            return *this;

        rc()++;

        return *this;
    }

    template <typename K> sptr<T> operator+(const K& operand)
    {
        return new_<T>(*ptr + operand);
    }

    template <typename K> sptr<T> operator-(const K& operand)
    {
        return new_<T>(*ptr - operand);
    }

    sptr<T> operator+()
    {
        return new_<T>(+(*ptr));
    }

    sptr<T> operator-()
    {
        return new_<T>(-(*ptr));
    }

    template <typename K> sptr<T> operator*(const K& operand)
    {
        return new_<T>(*ptr * operand);
    }

    template <typename K> sptr<T> operator/(const K& operand)
    {
        return new_<T>(*ptr / operand);
    }

    template <typename K> sptr<T> operator%(const K& operand)
    {
        return new_<T>(*ptr % operand);
    }

    sptr<T>& operator++()
    {
        ++(*ptr);
        return *this;
    }

//    sptr<T> operator++(int)
//    {
//        return new_<T>((*p)++);
//    }

    sptr<T>& operator--()
    {
        --(*ptr);
        return *this;
    }

//    sptr<T> operator--(int)
//    {
//        return new_<T>((*p)--);
//    }

    // Comparison operators/relational operators

    template <typename K> bool operator==(const K& operand)
    {
        return *ptr == operand;
    }

    template <typename K> bool operator!=(const K& operand)
    {
        return *ptr != operand;
    }

    template <typename K> bool operator>(const K& operand)
    {
        return *ptr > operand;
    }

    template <typename K> bool operator<(const K& operand)
    {
        return *ptr < operand;
    }

    template <typename K> bool operator>=(const K& operand)
    {
        return *ptr >= operand;
    }

    template <typename K> bool operator<=(const K& operand)
    {
        return *ptr <= operand;
    }

    // Logical operators (! is only for bool, && and || are not overloadable in C#)

//    sptr<T> operator!()
//    {
//        return new_<T>(!(*p));
//    }
//
//    template <typename K> sptr<T> operator&&(const K& operand)
//    {
//        return new_<T>(*p && operand);
//    }
//
//    template <typename K> sptr<T> operator||(const K& operand)
//    {
//        return new_<T>(*p || operand);
//    }

    // Bitwise operators

    sptr<T> operator~()
    {
        return new_<T>(~(*ptr));
    }

    template <typename K> sptr<T> operator&(const K& operand)
    {
        return new_<T>(*ptr & operand);
    }

    template <typename K> sptr<T> operator|(const K& operand)
    {
        return new_<T>(*ptr | operand);
    }

    template <typename K>
    sptr<T> operator^(const K& operand)
    {
        return new_<T>(*ptr ^ operand);
    }

    template <typename K>
    sptr<T> operator<<(const K& operand)
    {
        return new_<T>(*ptr << operand);
    }

    template <typename K> sptr<T> operator>>(const K& operand)
    {
        return new_<T>(*ptr >> operand);
    }

    // Compound assignment operators

    template <typename K> sptr<T>& operator+=(const K& operand)
    {
        *ptr += operand;
        return *this;
    }

    template <typename K> sptr<T>& operator-=(const K& operand)
    {
        *ptr -= operand;
        return *this;
    }

    template <typename K> sptr<T>& operator*=(const K& operand)
    {
        *ptr *= operand;
        return *this;
    }

    template <typename K> sptr<T>& operator/=(const K& operand)
    {
        *ptr /= operand;
        return *this;
    }

    template <typename K> sptr<T>& operator%=(const K& operand)
    {
        *ptr %= operand;
        return *this;
    }

    template <typename K> sptr<T>& operator&=(const K& operand)
    {
        *ptr &= operand;
        return *this;
    }

    template <typename K> sptr<T>& operator|=(const K& operand)
    {
        *ptr |= operand;
        return *this;
    }

    template <typename K> sptr<T>& operator^=(const K& operand)
    {
        *ptr ^= operand;
        return *this;
    }

    template <typename K> sptr<T>& operator<<=(const K& operand)
    {
        *ptr <<= operand;
        return *this;
    }

    template <typename K> sptr<T>& operator>>=(const K& operand)
    {
        *ptr >>= operand;
        return *this;
    }

    // Implicit conversion operators

    template <typename K> operator K()
    {
        return (K)(*ptr);
    }
};
}

// Give a special alias for sptr
template <typename T> using _ = sys::sptr<T>;

#pragma once

#include "../cspp.h"

template <typename T> struct _;

namespace sys
{
template <typename T> struct wptr
{
private:

    inline uint32_t& strong_rc()
    {
        return *((uint32_t*)ptr - 2);
    }

    inline uint32_t& weak_rc()
    {
        return *((uint32_t*)ptr - 1);
    }

public:

    T* ptr; // the size of wptr is same as a pointer

    wptr() : ptr(nullptr)
    {
    }

    wptr(const _<T>& copy) : ptr(copy.ptr)
    {
        if(!ptr)
            return;

        weak_rc()++;
    }

    wptr(const wptr<T>& copy) : ptr(copy.ptr)
    {
        if(!ptr)
            return;

        weak_rc()++;
    }

    ~wptr()
    {
        if(!ptr)
            return;

        uint32_t& wrc = weak_rc();
        wrc--;

        uint32_t rc = strong_rc();

        // When both strong and weak ref counter is zero, free the memory
        if(rc == 0 && wrc == 0)
        {
            free((void*)&rc);
        }

        // In all cases, since this wptr instance is destructed, ptr is set to null
        ptr = nullptr;
    }
};
}

template <typename T> struct _
{
private:

    inline uint32_t& strong_rc()
    {
        return *((uint32_t*)ptr - 2);
    }

    inline uint32_t& weak_rc()
    {
        return *((uint32_t*)ptr - 1);
    }

    void deref()
    {
        if(!ptr)
            return;

        uint32_t& rc = strong_rc();
        rc--;

        if(rc == 0)
        {
            try
            {
                // When strong ref counter is zero, destruct the object
                ptr->~T();
            }
            catch(...)
            {
                // It's a disaster when destructor throws... Log a bug and continue run
                // TODO: Log a bug for the destructor
            }

            // When both strong and weak ref counter is zero, free the memory
            if(weak_rc() == 0)
            {
                free((void*)&rc); // The memory block starts with the ref counter
            }
        }
    }

public:

    T* ptr; // the size of _ is same as a pointer

    _()
        : ptr(nullptr)
    {

    }

    _(T* p)
    {
        if(p == nullptr && ptr != nullptr)
        {
            deref();
        }

        ptr = p;

        if(!ptr)
            return;

        strong_rc() = 1;
    }

    _(const _<T>& copy)
        : ptr(copy.ptr)
    {
        if(!ptr)
            return;

        strong_rc()++;
    }

    _(const sys::wptr<T>& copy)
        : ptr(copy.ptr)
    {
        if(!ptr)
            return;

        uint32_t& rc = strong_rc();

        if(rc == 0)
        {
            // When a wptr is used, its possible that the object has already released
            // In this case, nothing is referenced, make it a empty _
            ptr = nullptr;
            return;
        }
        else
        {
            // The object still exists, increase ref counter since one more _ is holding the object
            rc++;
        }
    }

    ~_()
    {
        deref();

        // In all cases, since this _ instance is destructed, ptr is set to null
        ptr = nullptr;
    }

    // Operators pass-through. Refer to http://en.wikipedia.org/wiki/Operators_in_C_and_C%2B%2B
    // The restriction here is return type only support bool or _<T>

    // Member and pointer operators

    T* operator->()
    {
        return ptr;
    }

//    T operator&()
//    {
//        return *ptr;
//    }

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

    template <typename R> inline R& IndexOf(uint32_t i0, uint32_t i1 = 0, uint32_t i2 = 0, uint32_t i3 = 0, uint32_t i4 = 0, uint32_t i5 = 0, uint32_t i6 = 0, uint32_t i7 = 0, uint32_t i8 = 0, uint32_t i9 = 0)
    {
        return (*ptr)(i0, i1, i2, i3, i4, i5, i6, i7, i8, i9);
    }

    // Arithmetic operators

    _<T>& operator=(const _<T>& operand)
    {
        T* p = operand.ptr;
        if (p == nullptr)
        {
            deref();
        }

        ptr = p;

        if(!ptr)
            return *this;

        strong_rc()++;

        return *this;
    }

    template <typename K> _<T> operator+(const K& operand)
    {
        return new_<T>(*ptr + operand);
    }

    template <typename K> _<T> operator-(const K& operand)
    {
        return new_<T>(*ptr - operand);
    }

    _<T> operator+()
    {
        return new_<T>(+(*ptr));
    }

    _<T> operator-()
    {
        return new_<T>(-(*ptr));
    }

    template <typename K> _<T> operator*(const K& operand)
    {
        return new_<T>(*ptr * operand);
    }

    template <typename K> _<T> operator/(const K& operand)
    {
        return new_<T>(*ptr / operand);
    }

    template <typename K> _<T> operator%(const K& operand)
    {
        return new_<T>(*ptr % operand);
    }

    _<T>& operator++()
    {
        ++(*ptr);
        return *this;
    }

//    _<T> operator++(int)
//    {
//        return new_<T>((*p)++);
//    }

    _<T>& operator--()
    {
        --(*ptr);
        return *this;
    }

//    _<T> operator--(int)
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

//    _<T> operator!()
//    {
//        return new_<T>(!(*p));
//    }
//
//    template <typename K> _<T> operator&&(const K& operand)
//    {
//        return new_<T>(*p && operand);
//    }
//
//    template <typename K> _<T> operator||(const K& operand)
//    {
//        return new_<T>(*p || operand);
//    }

    // Bitwise operators

    _<T> operator~()
    {
        return new_<T>(~(*ptr));
    }

    template <typename K> _<T> operator&(const K& operand)
    {
        return new_<T>(*ptr & operand);
    }

    template <typename K> _<T> operator|(const K& operand)
    {
        return new_<T>(*ptr | operand);
    }

    template <typename K>
    _<T> operator^(const K& operand)
    {
        return new_<T>(*ptr ^ operand);
    }

    template <typename K>
    _<T> operator<<(const K& operand)
    {
        return new_<T>(*ptr << operand);
    }

    template <typename K> _<T> operator>>(const K& operand)
    {
        return new_<T>(*ptr >> operand);
    }

    // Compound assignment operators

    template <typename K> _<T>& operator+=(const K& operand)
    {
        *ptr += operand;
        return *this;
    }

    template <typename K> _<T>& operator-=(const K& operand)
    {
        *ptr -= operand;
        return *this;
    }

    template <typename K> _<T>& operator*=(const K& operand)
    {
        *ptr *= operand;
        return *this;
    }

    template <typename K> _<T>& operator/=(const K& operand)
    {
        *ptr /= operand;
        return *this;
    }

    template <typename K> _<T>& operator%=(const K& operand)
    {
        *ptr %= operand;
        return *this;
    }

    template <typename K> _<T>& operator&=(const K& operand)
    {
        *ptr &= operand;
        return *this;
    }

    template <typename K> _<T>& operator|=(const K& operand)
    {
        *ptr |= operand;
        return *this;
    }

    template <typename K> _<T>& operator^=(const K& operand)
    {
        *ptr ^= operand;
        return *this;
    }

    template <typename K> _<T>& operator<<=(const K& operand)
    {
        *ptr <<= operand;
        return *this;
    }

    template <typename K> _<T>& operator>>=(const K& operand)
    {
        *ptr >>= operand;
        return *this;
    }

    // Implicit conversion operators

    template <typename K> operator K()
    {
        return (K)(*ptr);
    }

    template <typename K> operator sys::wptr<K>()
    {
        return sys::wptr<K>(*this);
    }
};

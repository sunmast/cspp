#pragma once

#include "../cspp.h"

namespace sys
{

template <typename T> struct wptr;
template <typename T> struct sptr
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

    T* ptr; // the size of sptr is same as a pointer

    sptr()
        : ptr(nullptr)
    {

    }

    sptr(T* p)
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

    sptr(const sptr<T>& copy)
        : ptr(copy.ptr)
    {
        if(!ptr)
            return;

        strong_rc()++;
    }

    sptr(const wptr<T>& copy)
        : ptr(copy.ptr)
    {
        if(!ptr)
            return;

        uint32_t& rc = strong_rc();

        if(rc == 0)
        {
            // When a wptr is used, its possible that the object has already released
            // In this case, nothing is referenced, make it a empty sptr
            ptr = nullptr;
            return;
        }
        else
        {
            // The object still exists, increase ref counter since one more sptr is holding the object
            rc++;
        }
    }

    ~sptr()
    {
        deref();

        // In all cases, since this sptr instance is destructed, ptr is set to null
        ptr = nullptr;
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

    template <typename K> operator wptr<K>()
    {
        return wptr<K>(*this);
    }
};

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

    wptr(const sptr<T>& copy) : ptr(copy.ptr)
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

// Give a special alias for sptr (but not wptr)
template <typename T> using _ = sys::sptr<T>;

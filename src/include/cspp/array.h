#pragma once

#include "../cspp.h"

template <typename T> class array
{
private:
    size_t length;
    T* p;

public:
    array()
        : length(0)
        , p(nullptr)
    {
    }

    array(size_t _length)
        : length(_length)
        , p((T*)calloc(length, sizeof(T)))
    {
        if(!p)
        {
            // throw new OutOfMemoryException
        }
    }

    array(size_t _length, T* _p)
        : length(_length)
        , p(_p)
    {
        if(!_p)
        {
            // throw new ArugmentNullException
        }
    }

    ~array()
    {
        // TODO: delete elements if T is ref type (sptr)
        //printf("~array called \n");
    }

    inline size_t get_Length()
    {
        return length;
    }

    T& operator[](size_t index)
    {
        if(index >= length)
        {
            // throw new ArgumentOutOfRange exception
        }

        return p[index];
    }

    bool operator==(const array<T>& operand)
    {
        return this == &operand;
    }
};

template <typename T> class _array : public _<array<T>>
{
public:
    _array(array<T>* arr) : _<array<T>>(arr)
    {
    }
};

#pragma once

#include "../cspp.h"

template <typename T, uint32_t D> class _array;

template <typename T, uint32_t D = 1> class array
{
private:
    uint32_t d[D];
    T* ptr;

public:

    array()
    {
        d = {};
        ptr = nullptr;
    }

    array(uint32_t l0, uint32_t l1 = 0, uint32_t l2 = 0, uint32_t l3 = 0, uint32_t l4 = 0, uint32_t l5 = 0, uint32_t l6 = 0, uint32_t l7 = 0, uint32_t l8 = 0, uint32_t l9 = 0)
    {
        d[0] = l0;

        // The C++ compiler should be able to omit some of this according to the value of D
        // This supports up to 10 dimensions (even Interstellar has only 5 d...)
        // To support more dimensions, add l10, l11, l12, ...
        if (D > 1) d[1] = l1;
        if (D > 2) d[2] = l2;
        if (D > 3) d[3] = l3;
        if (D > 4) d[4] = l4;
        if (D > 5) d[5] = l5;
        if (D > 6) d[6] = l6;
        if (D > 7) d[7] = l7;
        if (D > 8) d[8] = l8;
        if (D > 9) d[9] = l9;

        ptr = (T*)calloc(get_Length(), sizeof(T));

        if(!ptr)
        {
            throw;
            // throw new OutOfMemoryException
        }
    }

    array(uint32_t _d[D], T* _ptr)
        : d(_d)
        , ptr(_ptr)
    {
        if(!_ptr)
        {
            throw;
            // throw new ArugmentNullException
        }
    }

    ~array()
    {
        // TODO: delete elements if T is ref type (sptr)
        //printf("~array called \n");
    }

    inline uint32_t get_Length()
    {
        if(D == 1)
        {
            return d[0];
        }
        else
        {
            uint32_t len = 1;
            for(int i = 0; i < D; i++)
            {
                len *= d[i];
            }

            return len;
        }
    }

    inline T& operator()(uint32_t i0, uint32_t i1 = 0, uint32_t i2 = 0, uint32_t i3 = 0, uint32_t i4 = 0, uint32_t i5 = 0, uint32_t i6 = 0, uint32_t i7 = 0, uint32_t i8 = 0, uint32_t i9 = 0)
    {
        uint32_t index = i0;

        // The C++ compiler should be able to omit some of this according to the value of D
        // This supports up to 10 dimensions (even Interstellar has only 5 d...)
        // To support more dimensions, add i10, i11, i12, ...
        if (D > 1) index = index * d[1] + i1;
        if (D > 2) index = index * d[2] + i2;
        if (D > 3) index = index * d[3] + i3;
        if (D > 4) index = index * d[4] + i4;
        if (D > 5) index = index * d[5] + i5;
        if (D > 6) index = index * d[6] + i6;
        if (D > 7) index = index * d[7] + i7;
        if (D > 8) index = index * d[8] + i8;
        if (D > 9) index = index * d[9] + i9;

        return ptr[index];
    }

    inline T& operator[](uint32_t index)
    {
        if(index >= d[0])
        {
            // throw new ArgumentOutOfRange exception
        }

        return ptr[index];
    }

//    inline T& operator[](uint32_t index1, uint32_t index2, uint32_t index3)
//    {
//        return ptr[index1, index2, index3];
//    }

    bool operator==(const array<T, D>& operand)
    {
        return this == &operand;
    }

    array* init(std::initializer_list<T> values)
    {
        // TODO: the compiler should be able to handle the initializer syntax so no need to use std::initializer_list
        T* p = ptr;
        for(auto val : values)
        {
            *p = val;
            p++;
        }

        return this;
    }

    void init(array<T, D>* values)
    {
        T* p = ptr;
        for(auto val : *values)
        {
            *p = val;
            p++;
        }
    }

    void init(_array<T, D> values)
    {
        init(values.ptr);
    }

    inline T* begin()
    {
        return ptr;
    }

    inline T* end()
    {
        return ptr + get_Length();
    }
};

template <typename T, uint32_t D = 1> class _array : public _<array<T, D>>
{
private:

public:
    _array(array<T, D>* arr) : _<array<T, D>>(arr)
    {
    }

    _array(std::initializer_list<T> values) : _<array<T, D>>(new_<array<T, D>>(values.size())->init(values))
    {
        // TODO: support multi-dimension array creation with initializer
    }
};

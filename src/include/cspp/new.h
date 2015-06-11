#pragma once

#include "../cspp.h"

// Always use calloc to zero memory to keep semantics same as C#
// Always allocate additional 8 bytes for the storage of ref counters
template <typename T> inline T* AllocateObject()
{
    void* p = calloc(1, sizeof(uint64_t) + sizeof(T));

    if(!p)
    {
        // throw new OutOfMemoryException();
    }

    // The memory layout will be: [strong ref counter: 4 bytes][weak ref counter: 4 bytes][object: sizeof(T) bytes]
    return (T*)((uint64_t*)p + 1);
}

// Support up to 10 parameters for constructors
template <typename T> inline T* new_()
{
    void* p = AllocateObject<T>();
    return new (p) T();
}

template <typename T, typename P1> inline T* new_(P1 p1)
{
    void* p = AllocateObject<T>();
    return new (p) T(p1);
}

template <typename T, typename P1, typename P2> inline T* new_(P1 p1, P2 p2)
{
    void* p = AllocateObject<T>();
    return new (p) T(p1, p2);
}

template <typename T, typename P1, typename P2, typename P3> inline T* new_(P1 p1, P2 p2, P3 p3)
{
    void* p = AllocateObject<T>();
    return new (p) T(p1, p2, p3);
}

template <typename T, typename P1, typename P2, typename P3, typename P4> inline T* new_(P1 p1, P2 p2, P3 p3, P4 p4)
{
    void* p = AllocateObject<T>();
    return new (p) T(p1, p2, p3, p4);
}

template <typename T, typename P1, typename P2, typename P3, typename P4, typename P5>
inline T* new_(P1 p1, P2 p2, P3 p3, P4 p4, P5 p5)
{
    void* p = AllocateObject<T>();
    return new (p) T(p1, p2, p3, p4, p5);
}

template <typename T, typename P1, typename P2, typename P3, typename P4, typename P5, typename P6>
inline T* new_(P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6)
{
    void* p = AllocateObject<T>();
    return new (p) T(p1, p2, p3, p4, p5, p6);
}

template <typename T, typename P1, typename P2, typename P3, typename P4, typename P5, typename P6, typename P7>
inline T* new_(P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7)
{
    void* p = AllocateObject<T>();
    return new (p) T(p1, p2, p3, p4, p5, p6, p7);
}

template <typename T,
         typename P1,
         typename P2,
         typename P3,
         typename P4,
         typename P5,
         typename P6,
         typename P7,
         typename P8>
inline T* new_(P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8)
{
    void* p = AllocateObject<T>();
    return new (p) T(p1, p2, p3, p4, p5, p6, p7, p8);
}

template <typename T,
         typename P1,
         typename P2,
         typename P3,
         typename P4,
         typename P5,
         typename P6,
         typename P7,
         typename P8,
         typename P9>
inline T* new_(P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9)
{
    void* p = AllocateObject<T>();
    return new (p) T(p1, p2, p3, p4, p5, p6, p7, p8, p9);
}

template <typename T,
         typename P1,
         typename P2,
         typename P3,
         typename P4,
         typename P5,
         typename P6,
         typename P7,
         typename P8,
         typename P9,
         typename P10>
inline T* new_(P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8, P9 p9, P10 p10)
{
    void* p = AllocateObject<T>();
    return new (p) T(p1, p2, p3, p4, p5, p6, p7, p8, p9, p10);
}

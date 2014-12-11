namespace Archient.Testing

module Xunit =
    
    open System

    open Xunit

    let inline assertTrue condition =
        ignore <| Assert.True(condition)

    let inline assertIsTrue (test:'t->bool) (context:'t) =
        assertTrue (test context)
        context

    let inline assertFalse condition =
        ignore <| Assert.False(condition)

    let inline assertIsFalse (test:'t->bool) (context:'t) =
        assertFalse (test context)
        context
    
    let inline assertNull value =
        ignore <| Assert.Null(value)
    
    let inline assertIsNull (f:'t->'a) (context:'t) =
        assertNull (f context)
        context

    let inline assertNotNull value =
        ignore <| Assert.NotNull(value)
    
    let inline assertIsNotNull (f:'t->'a) (context:'t) =
        assertNotNull (f context)
        context
    
    let inline assertEqual (expected:'t) (actual:'t) =
        ignore <| Assert.Equal<'t>(expected, actual)
    
    let inline assertAreEqual (expected:'a) (f:'t->'a) (context:'t) =
        assertEqual expected (f context)
        context
    
    let inline assertNotEqual (expected:'t) (actual:'t) =
        ignore <| Assert.NotEqual<'t>(expected, actual)
    
    let inline assertAreNotEqual (expected:'a) (f:'t->'a) (context:'t) =
        assertNotEqual expected (f context)
        context
    
    let inline assertSame (expected:'t) (actual:'t) =
        ignore <| Assert.Same(expected, actual)
    
    let inline assertAreSame (expected:'a) (f:'t->'a) (context:'t) =
        assertSame expected (f context)
        context
    
    let inline assertNotSame (expected:'t) (actual:'t) =
        ignore <| Assert.NotSame(expected, actual)
    
    let inline assertAreNotSame (expected:'a) (f:'t->'a) (context:'t) =
        assertNotSame expected (f context)
        context

    let inline dispose<'t when 't :> IDisposable> (value:'t) =
        value.Dispose()
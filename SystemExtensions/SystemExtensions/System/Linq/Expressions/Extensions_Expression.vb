Imports System
Imports System.Collections
Imports System.ComponentModel
Imports System.Diagnostics
Imports System.Linq
Imports System.Reflection
Imports System.Runtime.CompilerServices
Imports System.Text

Namespace System.Linq.Expressions

  Public Module ExtensionsForExpression

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function [AndAlso](Of TEntity)(one As Expression(Of Func(Of TEntity, Boolean)), another As Expression(Of Func(Of TEntity, Boolean))) As Expression(Of Func(Of TEntity, Boolean))
      Dim combinedBody = Expression.AndAlso(one.Body, another.Body)
      Return Expression.Lambda(Of Func(Of TEntity, Boolean))(combinedBody, one.Parameters(0))
    End Function

    <Extension(), EditorBrowsable(EditorBrowsableState.Always)>
    Public Function [OrElse](Of TEntity)(one As Expression(Of Func(Of TEntity, Boolean)), another As Expression(Of Func(Of TEntity, Boolean))) As Expression(Of Func(Of TEntity, Boolean))
      Dim combinedBody = Expression.OrElse(one.Body, another.Body)
      Return Expression.Lambda(Of Func(Of TEntity, Boolean))(combinedBody, one.Parameters(0))
    End Function

  End Module

End Namespace


Imports System
Imports System.Linq
Imports System.ComponentModel

Namespace System.Collections.Generic

  Public Interface IQueryableSetContainer

    Function [Set](Of T As Class)() As IQueryableSet(Of T)

  End Interface


  Public Interface ITransactionalQueryableSetContainer
    Inherits IQueryableSetContainer
    Inherits IDisposable

    Sub CommitChanges()
    Sub RollbackChanges()

  End Interface

End Namespace

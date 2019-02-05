Imports System
Imports System.Collections
Imports System.ComponentModel
Imports System.Diagnostics
Imports System.Linq
Imports System.Reflection
Imports System.Runtime.CompilerServices
Imports System.Text

Namespace System

  Public Interface IDisposableEx
    Inherits IDisposable

    <EditorBrowsable(EditorBrowsableState.Advanced)>
    Event Disposed As EventHandler

    <EditorBrowsable(EditorBrowsableState.Advanced)>
    ReadOnly Property IsDisposed As Boolean

  End Interface

End Namespace

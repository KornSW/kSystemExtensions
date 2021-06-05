Imports System.Runtime.CompilerServices
Imports System.ComponentModel
Imports System.Text
Imports System

Namespace System

  ''' <summary>
  ''' Supports to be 'sealed' (that means, that an object instance can be set to ReadOnly mode)
  ''' </summary>
  Public Interface ISealable

    ''' <summary>
    ''' Set this instance to ReadOnly mode 
    ''' </summary>
    Sub Seal()

    ''' <summary>
    ''' Indicates, that this instance was set to ReadOnly mode
    ''' </summary>
    ReadOnly Property IsSealed As Boolean

  End Interface

End Namespace

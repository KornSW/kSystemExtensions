Imports System.Runtime.CompilerServices
Imports System.ComponentModel
Imports System.Text
Imports System

Namespace System

  ''' <summary>
  ''' Contains an internal cache, which can be invalidated und rebuilded
  ''' </summary>
  Public Interface ICache

    ''' <summary>
    ''' Invalidates the cache without rebuilding it (it will be rebuilded within the next access)
    ''' </summary>
    Sub Invalidate()

    ''' <summary>
    ''' Indicates, that the cache is currently invalidated and will be rebuilded within the next access
    ''' </summary>
    ReadOnly Property IsInvalidated As Boolean

    ''' <summary>
    ''' Builds the cache now (use this, if you dont want an on demand caching)
    ''' </summary>
    ''' <param name="forceRebuild">use this option to force an rebuild of the cache, also if it is currently not invalid.</param>
    Sub BuildCache(forceRebuild As Boolean)

  End Interface

End Namespace

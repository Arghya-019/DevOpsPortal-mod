' Ok, I learned how to do this by trying to get BradleyB's 'How to auto-increment assembly version using a custom MSBuild taskworking
'and I failed horribly.
'http://weblogs.asp.net/bradleyb/archive/2005/12/02/432150.aspx

'So I set out to make my own system, using what I learned about Build Tasks from the above sample.

'I eventually got it working from that code, and what I learned from the source code
'provided in http://blogs.msdn.com/msbuild/archive/2005/11/11/491947.aspx

'While hte MSBuild Team Blog teams code worked, it overwrote the major and minor numbers
'and didn't do "exactly" what I wanted... plus their source code was overly complicated.






'So I wrote this dll. To get it working, do the below steps.

'1. Copy the BuildTask.dll into the same folder that your project file is in. (.vbproj for vb)


'2. In your project file itself add this line:

'<UsingTask TaskName="BuildTasks.IncrementBuildNumber" AssemblyFile="BuildTask.dll" />

'between the <Project> tag  and first <PropertyGroup> tag.

'ie: the first three lines of my test project look like this:
'	<Project DefaultTargets="Build" xmlns="http://schemas.microsoft.com/developer/msbuild/2003">
'		<UsingTask TaskName="BuildTasks.IncrementBuildNumber" AssemblyFile="BuildTask.dll" />
'		<PropertyGroup>


'3. Now, find hte <Target Name="BeforeBuild"></Target> tags and add this between them.
'(Make sure they get uncommented if they are - the <!-- and --> tags workt he same as /* */ in c#

'   <IncrementBuildNumber
'     AssemblyFileLocation = "$(MSBuildProjectDirectory)\My Project\AssemblyInfo.vb">
'   </IncrementBuildNumber>


'IMPORTANT!  - Make sure the path to your assembly file is CORRECT!
'$(MSBuildProjectDirectory) means the folder your project file is in, so it will be relative tot hat
'in my case, it's in the My Project folder, and since it's a vb app, it's the AssemblyInfo.vb



'4. Save the project and load it. The major and minor numbers will will be whatever you assign in the
'assembly file, but the build and revision will be overwritten.




'The default that I use is   MAJOR.MINOR.Number of days since jan 01 2000.Number of times compiled today

'When you run/compile the revision number increases. This will occure EVERY TIME the dll is called.
'so if your visual studio calls it multiple times, your number could jump 2 or 3 or more at a time.
'I don't care, so I never bothered to try to fix this.

'Also, the revision number *may* not increament if you have the assembly or project properties window open
'Again, I didn't care enough to fix it as it's not that big a deal.


'Enjoy.

'Dave Gallant
'kaydeng@yahoo.ca

Imports System
Imports Microsoft.Build.Framework
Imports Microsoft.Build.Utilities

Namespace BuildTasks
    Public Class IncrementBuildNumber
        Inherits Task

        Dim m_AssemblyFileLocation As String ' This is the Assembly file that has the version info.

        Public Overrides Function Execute() As Boolean
            Try
                Return IncrementVersion()
            Catch ex As Exception
                Log.LogError(ex.Message)
                Return False
            End Try
        End Function 'Execute 

        <Required()> Public Property AssemblyFileLocation() As String
            Get
                Return m_AssemblyFileLocation
            End Get
            Set(ByVal value As String)
                m_AssemblyFileLocation = value
            End Set
        End Property

        Private Function IncrementVersion() As Boolean

            Dim i As Integer
            Dim FileData() As String
            Dim s As String
            Dim Version As String
            Dim v() As String
            Dim ResetRevision As Boolean = True

            ' All the commented msgbox lines are for debugging.
            ' you don't need them.

            'If MsgBox("IncrementVersion Run on file " & m_AssemblyFileLocation & vbNewLine & vbNewLine & "Copy path to clipboard?", MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
            '    My.Computer.Clipboard.Clear()
            '    My.Computer.Clipboard.SetText(m_AssemblyFileLocation)
            'End If


            ' Ok, we have the assembly file, try to open it.
            If IO.File.Exists(m_AssemblyFileLocation) Then

                ' Set build number to the number of days that have passed since Jan 1 2000
                ' If you want to modifiy what the build number means or does, here is the place.
                Dim Major As Integer = 1
                Dim Minor As Integer = 0
                Dim BuildNumber As Integer = Math.Abs(DateDiff(DateInterval.Day, Now.Date, CType("JAN/01/2000", Date)))
                Dim Revision As Integer = 0

                'MsgBox("New build number = " & BuildNumber)

                Try
                    FileData = IO.File.ReadAllLines(m_AssemblyFileLocation)

                    'MsgBox("Read " & FileData.Length & " lines from " & m_AssemblyFileLocation)

                    If FileData.Length = 0 Then Exit Function
                    ' loop through each line and look for the version lines.
                    For i = 0 To FileData.Length - 1
                        s = FileData(i)
                        If s.Length > 2 Then
                            ' Look to see if it contains one of the 2 version lines we want.
                            'VB: <Assembly: AssemblyVersion("0.0.0.0")> 
                            'VB: <Assembly: AssemblyFileVersion("0.0.0.0")> 
                            'C#: [assembly: AssemblyFileVersion("1.0.0.0")]
                            'C#: [assembly: AssemblyVersion("1.0.0.0")]
                            If Not s.Substring(0, 1) = "'" And Not s.Substring(0, 2) = "//" Then

                                If s.Contains("AssemblyVersion") Or s.Contains("AssemblyFileVersion") Then

                                    'MsgBox("Target line " & s & " found, parsing now.")

                                    ' Get the version from the line.
                                    ' we do this by getting the first " and losing everything before it.
                                    ' do the same after the next "
                                    ' everything left should be version info.
                                    Version = Microsoft.VisualBasic.Right(s, s.Length - s.IndexOf(Chr(34)) - 1)
                                    Version = Microsoft.VisualBasic.Left(Version, Version.IndexOf(Chr(34)))

                                    'MsgBox("Version found = " & Version)

                                    v = Version.Split(".")
                                    If v.Length >= 0 Then Major = Val(v(0))
                                    If v.Length >= 1 Then Minor = Val(v(1))
                                    If v.Length >= 2 Then ResetRevision = (BuildNumber <> Val(v(2)))
                                    If v.Length >= 3 Then Revision = Val(v(3)) + 1

                                    ' ok, now that we have the version numbers in their variables
                                    ' it's time to update the build, if needed.
                                    If ResetRevision Then Revision = 1

                                    'MsgBox("Replacing version with " & Major & "." & Minor & "." & BuildNumber & "." & Revision)

                                    ' ok, update the original line from the array.
                                    FileData(i) = FileData(i).Replace(Version, Major & "." & Minor & "." & BuildNumber & "." & Revision)

                                    'MsgBox("Filedata(" & i & ") = " & FileData(i))
                                End If
                            End If
                        End If
                    Next

                    ' ok, rewrite the assembly info back into the file, and let's home the compiler picks it up
                    'MsgBox("writing all data back to file")
                    IO.File.WriteAllLines(m_AssemblyFileLocation, FileData)


                Catch ex As Exception
                    ' hrm. Error. Fail please.
                    MsgBox("ERROR! " & ex.Message, "Build Tasks")
                    Log.LogError(ex.Message)
                    Return False
                End Try
            End If

            ' return success
            Return True
        End Function

    End Class
End Namespace
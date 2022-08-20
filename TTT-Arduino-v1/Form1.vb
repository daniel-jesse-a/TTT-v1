Imports System.IO
Imports System.IO.Ports
Imports System.Globalization
Imports System.Net
Imports System.Net.Sockets
Imports System.Text
Imports System.Drawing

Public Class Form1

    Dim running As Boolean = False
    Dim startTime As DateTime
    Dim counter As Integer = 0

    Dim winner As String = 20
    Dim loser As String = 30
    Dim play As String = 40

    Dim buttons As Button() = {Button1, Button2, Button3, Button4, Button5, Button6, Button7, Button8, Button9}
    Dim buttoncheck As Button()

    Dim _client As TcpClient
    Public Function isWinner(ByVal key As Char)
        Return ((Button1.Text = Button2.Text And Button2.Text = Button3.Text And Button3.Text = key) Or
            (Button4.Text = Button5.Text And Button5.Text = Button6.Text And Button6.Text = key) Or
            (Button7.Text = Button8.Text And Button8.Text = Button9.Text And Button9.Text = key) Or
            (Button1.Text = Button5.Text And Button5.Text = Button9.Text And Button9.Text = key) Or
            (Button3.Text = Button5.Text And Button5.Text = Button7.Text And Button7.Text = key) Or
            (Button1.Text = Button4.Text And Button4.Text = Button7.Text And Button7.Text = key) Or
            (Button2.Text = Button5.Text And Button5.Text = Button8.Text And Button8.Text = key) Or
            (Button3.Text = Button6.Text And Button6.Text = Button9.Text And Button9.Text = key))
        Timer1.Stop()

    End Function


    Public Function ComputerInput()
        Dim btns As Button() = {Button1, Button2, Button3, Button4, Button5, Button6, Button7, Button8, Button9}
        Dim possInput As List(Of Button) = New List(Of Button)

        Dim ns As NetworkStream = _client.GetStream()
        Dim comp As String

        For j As Integer = 0 To btns.Length - 1
            If (btns(j).Text = "") Then
                possInput.Add(btns(j))
            End If
        Next

        Dim chs() As Char = {"X", "O"}

        For Each ch As Char In chs
            For Each btn As Button In possInput
                btn.Text = ch
                If isWinner(ch) Then
                    btn.Text = "X"
                    comp = btn.Name.Substring(6)
                    Label3.Text = comp
                    ns.Write(Encoding.ASCII.GetBytes(comp), 0, comp.Length)
                    btn.Enabled = False
                    Return (True)
                End If
                btn.Text = ""
            Next
        Next

        Dim cornerBtn As List(Of Button) = New List(Of Button)

        For Each btn As Button In possInput
            If (btn Is Button1 Or btn Is Button3 Or btn Is Button7 Or btn Is Button9) Then
                cornerBtn.Add(btn)
            End If
        Next

        Dim tempBtn As Button
        If cornerBtn.Count > 0 Then
            tempBtn = cornerBtn(New Random().Next(0, cornerBtn.Count - 1))
            tempBtn.Text = "X"
            comp = tempBtn.Name.Substring(6)
            Label3.Text = comp
            ns.Write(Encoding.ASCII.GetBytes(comp), 0, comp.Length)
            tempBtn.Enabled = False
            Return (True)
        End If

        If possInput.Contains(Button5) Then
            Button5.Text = "X"
            Button5.Enabled = False
            Return (True)
        End If

        Dim edgeBtn As List(Of Button) = New List(Of Button)
        For Each btn As Button In possInput
            If (btn Is Button2 Or btn Is Button4 Or btn Is Button6 Or btn Is Button8) Then
                edgeBtn.Add(btn)
            End If
        Next

        If edgeBtn.Count > 0 Then
            tempBtn = edgeBtn(New Random().Next(0, edgeBtn.Count - 1))
            tempBtn.Text = "X"
            tempBtn.Enabled = False
            Return (True)
        End If

        Return (False)
    End Function

    Public Sub reset()
        Dim btns As Button() = {Button1, Button2, Button3, Button4, Button5, Button6, Button7, Button8, Button9}
        For Each btn As Button In btns
            btn.Text = ""
            btn.Enabled = True
            btn.BackColor = DefaultBackColor

        Next
    End Sub

    Public Function checkTheWinner(ByVal key As Char)
        If (Button1.Text = Button2.Text And Button2.Text = Button3.Text And Button3.Text = key) Then
            Button1.BackColor = Color.GreenYellow
            Button2.BackColor = Color.GreenYellow
            Button3.BackColor = Color.GreenYellow
        ElseIf (Button4.Text = Button5.Text And Button5.Text = Button6.Text And Button6.Text = key) Then
            Button4.BackColor = Color.GreenYellow
            Button5.BackColor = Color.GreenYellow
            Button6.BackColor = Color.GreenYellow
        ElseIf (Button7.Text = Button8.Text And Button8.Text = Button9.Text And Button9.Text = key) Then
            Button7.BackColor = Color.GreenYellow
            Button8.BackColor = Color.GreenYellow
            Button9.BackColor = Color.GreenYellow
        ElseIf (Button1.Text = Button5.Text And Button5.Text = Button9.Text And Button9.Text = key) Then
            Button1.BackColor = Color.GreenYellow
            Button5.BackColor = Color.GreenYellow
            Button9.BackColor = Color.GreenYellow
        ElseIf (Button3.Text = Button5.Text And Button5.Text = Button7.Text And Button7.Text = key) Then
            Button3.BackColor = Color.GreenYellow
            Button5.BackColor = Color.GreenYellow
            Button7.BackColor = Color.GreenYellow
        ElseIf (Button1.Text = Button4.Text And Button4.Text = Button7.Text And Button7.Text = key) Then
            Button1.BackColor = Color.GreenYellow
            Button4.BackColor = Color.GreenYellow
            Button7.BackColor = Color.GreenYellow
        ElseIf (Button2.Text = Button5.Text And Button5.Text = Button8.Text And Button8.Text = key) Then
            Button2.BackColor = Color.GreenYellow
            Button5.BackColor = Color.GreenYellow
            Button8.BackColor = Color.GreenYellow
        ElseIf (Button3.Text = Button6.Text And Button6.Text = Button9.Text And Button9.Text = key) Then
            Button3.BackColor = Color.GreenYellow
            Button6.BackColor = Color.GreenYellow
            Button9.BackColor = Color.GreenYellow
        Else
            Return (False)
        End If
        Return (True)
    End Function
    Public Sub check()
        Dim ns As NetworkStream = _client.GetStream()
        If checkTheWinner("O") Then
            ns.Write(Encoding.ASCII.GetBytes(winner), 0, winner.Length)
            Button11.Text = "You Won"
            Button11.BackColor = Color.GreenYellow
            Button11.Enabled = False
            Button10.Enabled = True
            Timer1.Stop()
            Timer1.Enabled = False
        ElseIf checkTheWinner("X") Then
            ns.Write(Encoding.ASCII.GetBytes(loser), 0, loser.Length)
            Button11.Text = "You Lose"
            Button11.BackColor = Color.Red
            Button11.Enabled = False
            Button10.Enabled = True
            Timer1.Stop()
            Timer1.Enabled = False
        Else
            Dim btns As Button() = {Button1, Button2, Button3, Button4, Button5, Button6, Button7, Button8, Button9}
            Dim possInput As List(Of Button) = New List(Of Button)

            For j As Integer = 0 To btns.Length - 1
                If (btns(j).Text = "") Then
                    possInput.Add(btns(j))
                End If
            Next
            If possInput.Count = 0 Then
                Button11.Text = "Match Tie"
                Button11.BackColor = Color.Yellow
                Button11.Enabled = False
                Button10.Enabled = True
                Timer1.Stop()
                Timer1.Enabled = False
            End If

            For j As Integer = 0 To btns.Length - 1
                If (btns(j).Text = "") Then
                    possInput.Add(btns(j))
                End If
            Next
        End If

    End Sub
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Button1.Text = "O"
        Button1.Enabled = False
        ComputerInput()
        check()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Button2.Text = "O"
        Button2.Enabled = False
        ComputerInput()
        check()
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Button3.Text = "O"
        Button3.Enabled = False
        ComputerInput()
        check()
    End Sub


    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        Button4.Text = "O"
        Button4.Enabled = False
        ComputerInput()
        check()
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        Button5.Text = "O"
        Button5.Enabled = False
        ComputerInput()
        check()
    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        Button6.Text = "O"
        Button6.Enabled = False
        ComputerInput()
        check()
    End Sub

    Private Sub Button7_Click(sender As Object, e As EventArgs) Handles Button7.Click
        Button7.Text = "O"
        Button7.Enabled = False
        ComputerInput()
        check()
    End Sub

    Private Sub Button8_Click(sender As Object, e As EventArgs) Handles Button8.Click
        Button8.Text = "O"
        Button8.Enabled = False
        ComputerInput()
        check()
    End Sub

    Private Sub Button9_Click(sender As Object, e As EventArgs) Handles Button9.Click
        Button9.Text = "O"
        Button9.Enabled = False
        ComputerInput()
        check()
    End Sub

    Private Sub Button11_Click(sender As Object, e As EventArgs) Handles Button11.Click
        reset()
        Button10.Enabled = True
        Timer1.Stop()
        counter = 0
    End Sub

    Private Sub Button10_Click(sender As Object, e As EventArgs) Handles Button10.Click
        Dim ns As NetworkStream = _client.GetStream()
        ns.Write(Encoding.ASCII.GetBytes(play), 0, play.Length)
        reset()
        Button11.Text = "Reset"
        Button11.BackColor = DefaultBackColor
        Button11.Enabled = True
        Button10.Enabled = False
        counter = 0

    End Sub

    Private Sub Buttons_Click(sender As Object, e As EventArgs) Handles Button1.Click, Button2.Click, Button3.Click, Button4.Click, Button5.Click, Button6.Click, Button7.Click, Button8.Click, Button9.Click
        counter += 1

        If counter < 2 Then
            startTime = DateTime.Now
            Timer1.Enabled = True
            Timer1.Start()
        End If


    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.CenterToParent()
        Windows.Forms.Control.CheckForIllegalCrossThreadCalls = False
        'Change COM Port Here
        SerialPort1.PortName = "COM8"
        SerialPort1.BaudRate = 9600
        SerialPort1.Open()
        Timer1.Stop()
        Label2.Text = "00:00:000"

    End Sub

    Private Sub SerialPort1_DataReceived(sender As Object, e As SerialDataReceivedEventArgs) Handles SerialPort1.DataReceived
        Dim SrlInp As String = SerialPort1.ReadLine

        counter += 1
        Label1.Text = counter

        If SrlInp = 1 Then
            Me.Button1.PerformClick()
        ElseIf SrlInp = 2 Then
            Me.Button2.PerformClick()
        ElseIf SrlInp = 3 Then
            Me.Button3.PerformClick()
        ElseIf SrlInp = 4 Then
            Me.Button4.PerformClick()
        ElseIf SrlInp = 5 Then
            Me.Button5.PerformClick()
        ElseIf SrlInp = 6 Then
            Me.Button6.PerformClick()
        ElseIf SrlInp = 7 Then
            Me.Button7.PerformClick()
        ElseIf SrlInp = 8 Then
            Me.Button8.PerformClick()
        ElseIf SrlInp = 9 Then
            Me.Button9.PerformClick()
        End If

    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick

        Dim diff As TimeSpan = DateTime.Now - startTime

        Label2.Text = diff.ToString("mm\:ss\:fff")

    End Sub

    Private Sub Button12_Click(sender As Object, e As EventArgs) Handles Button12.Click
        Try
            Dim ip As String = "192.168.5.1"
            Dim port As Integer = "4096"

            _client = New TcpClient(ip, port)

            CheckForIllegalCrossThreadCalls = False

            Threading.ThreadPool.QueueUserWorkItem(AddressOf ReceiveMessages)



        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub ReceiveMessages(state As Object)
        Try
            While True

                Dim ns As NetworkStream = _client.GetStream

                Dim toReceive(100000) As Byte
                ns.Read(toReceive, 0, toReceive.Length)
                Dim txt As String = Encoding.ASCII.GetString(toReceive)

                If RichTextBox1.TextLength > 0 Then
                    RichTextBox1.Text &= vbNewLine & txt
                Else
                    RichTextBox1.Text = txt
                End If

                If txt = 50 Then
                    Label4.Text = "Status: Connected"

                End If

                If txt = 9 Then
                    Label4.Text = "Status: Disonnected"
                End If

            End While

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub
End Class

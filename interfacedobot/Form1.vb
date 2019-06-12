Imports System.Net
Imports System.IO
Imports System.Net.Sockets
Imports System.ComponentModel

Public Class Form1
    Dim Receivenya As Threading.Thread
    Dim PC_Client As Sockets.TcpClient
    'Dim RX As StreamReader
    'Dim TX As StreamWriter
    Dim Ipadd As String
    Dim comport As String
    Dim client_stream As NetworkStream
    Private Delegate Sub switchVDelegate(ByVal text As String)
    Private Sub switchV(ByVal text As String)
        If BunifuSwitch1.InvokeRequired Then
            BunifuSwitch1.Invoke(New switchVDelegate(AddressOf switchV), text)
        Else
            BunifuSwitch1.Value = text
        End If
    End Sub
    Private Delegate Sub buff1Delegate(ByVal text As String)
    Private Sub buff1(ByVal text As String)
        If Text00.InvokeRequired Then
            Text00.Invoke(New buff1Delegate(AddressOf buff1), text)
        Else
            Text00.Clear()
            Text00.Text = text
        End If
    End Sub
    Private Delegate Sub buffDelegate(ByVal text As String)
    Private Sub buff(ByVal text As String)
        If Label15.InvokeRequired Then
            Label15.Invoke(New buffDelegate(AddressOf buff), text)
        Else
            Label15.Text = text
        End If
    End Sub
    Private Delegate Sub recv_dataDelegate(ByVal text As String)
    Private Sub recv_data(ByVal text As String)
        If recv_dat.InvokeRequired Then
            recv_dat.Invoke(New recv_dataDelegate(AddressOf recv_data), text)
        Else
            recv_dat.Clear()
            recv_dat.Text = text
        End If
    End Sub
    Private Delegate Sub paramXDelegate(ByVal text As String)
    Private Sub paramX(ByVal text As String)
        If TextX.InvokeRequired Then
            TextX.Invoke(New paramXDelegate(AddressOf paramX), text)
        Else
            TextX.Clear()
            TextX.Text = text
        End If
    End Sub
    Private Delegate Sub paramYDelegate(ByVal text As String)
    Private Sub paramY(ByVal text As String)
        If TextY.InvokeRequired Then
            TextY.Invoke(New paramYDelegate(AddressOf paramY), text)
        Else
            TextY.Clear()
            TextY.Text = text
        End If
    End Sub
    Private Delegate Sub paramZDelegate(ByVal text As String)
    Private Sub paramZ(ByVal text As String)
        If TextZ.InvokeRequired Then
            TextZ.Invoke(New paramZDelegate(AddressOf paramZ), text)
        Else
            TextZ.Clear()
            TextZ.Text = text
        End If
    End Sub
    Private Delegate Sub paramRDelegate(ByVal text As String)
    Private Sub paramR(ByVal text As String)
        If TextR.InvokeRequired Then
            TextR.Invoke(New paramRDelegate(AddressOf paramR), text)
        Else
            TextR.Clear()
            TextR.Text = text
        End If
    End Sub
    Private Delegate Sub paramJ1Delegate(ByVal text As String)
    Private Sub paramJ1(ByVal text As String)
        If TextJ1.InvokeRequired Then
            TextJ1.Invoke(New paramJ1Delegate(AddressOf paramJ1), text)
        Else
            TextJ1.Clear()
            TextJ1.Text = text
        End If
    End Sub
    Private Delegate Sub paramJ2Delegate(ByVal text As String)
    Private Sub paramJ2(ByVal text As String)
        If TextJ2.InvokeRequired Then
            TextJ2.Invoke(New paramJ2Delegate(AddressOf paramJ2), text)
        Else
            TextJ2.Clear()
            TextJ2.Text = text
        End If
    End Sub
    Private Delegate Sub paramJ3Delegate(ByVal text As String)
    Private Sub paramJ3(ByVal text As String)
        If TextJ3.InvokeRequired Then
            TextJ3.Invoke(New paramJ3Delegate(AddressOf paramJ3), text)
        Else
            TextJ3.Clear()
            TextJ3.Text = text
        End If
    End Sub
    Private Delegate Sub paramJ4Delegate(ByVal text As String)
    Private Sub paramJ4(ByVal text As String)
        If TextJ4.InvokeRequired Then
            TextJ4.Invoke(New paramJ4Delegate(AddressOf paramJ4), text)
        Else
            TextJ4.Clear()
            TextJ4.Text = text
        End If
    End Sub

    Private Sub Form1_Shown(sender As Object, e As EventArgs) Handles Me.Shown
        CheckForIllegalCrossThreadCalls = True
    End Sub
    '--------------------------Connection----------------------'
    Private Sub BCON_Click(sender As Object, e As EventArgs) Handles BCON.Click
        If BCON.Text = "CONNECT" Then
            comport = 5051
            Ipadd = TextBox2.Text
            If TextBox2.Text = "" Then
                MessageBox.Show("ISI IP DAN PORT!! ", "ATTENTION!!!", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                Try
                    PC_Client = New TcpClient(Ipadd, comport)
                    If PC_Client.GetStream.CanRead = True Then
                        Threading.ThreadPool.QueueUserWorkItem(AddressOf ReceiveMessages)
                        con_stat.Text = "Connected"
                        con_stat.ForeColor = System.Drawing.Color.FromArgb(80, 242, 43)
                        BCON.Text = "DISCONNECT"
                        send("SPEEDM 30")
                        'Bmanual.Enabled = True
                        'Bauto.Enabled = True
                        'Baboutus.Enabled = True
                    End If
                Catch ex As Exception
                    MessageBox.Show("IP ERROR " + ex.Message, "Attention!!!", MessageBoxButtons.OK, MessageBoxIcon.Information)
                End Try
            End If
        ElseIf BCON.Text = "DISCONNECT" Then
            send("EXIT")

            BCON.Text = "CONNECT"
            con_stat.Text = "disconnected"
            con_stat.ForeColor = System.Drawing.Color.Red
            Label38.Text = 30
            TrackBar1.Value = 30
            recv_dat.Clear()


            'Bmanual.Enabled = False
            'Bauto.Enabled = False
            'Baboutus.Enabled = False
        End If
    End Sub
    Private Sub ReceiveMessages(state As Object)
        'data max 51'
        Dim toReceive(60) As Byte
        Dim received As String = ""
        Dim kalimat() As String
        Dim Bfacum() As String
        Dim titikdua() As Char = {":"c}
        Dim kress() As Char = {"#"c}
        Try
            client_stream = PC_Client.GetStream()
            While True
                If client_stream.DataAvailable = True Then
                    client_stream.Read(toReceive, 0, toReceive.Length)
                    received = System.Text.Encoding.ASCII.GetString(toReceive)
                    recv_data(received)
                End If
                If received <> "" Then
                    If received.Contains("connected.") Then
                        MsgBox("connected!", MsgBoxStyle.OkOnly, "Rasberry PI Connection")
                    End If
                    If received.Contains("K") Then
                        kalimat = recv_dat.Text.Split(titikdua)
                        paramX(kalimat(1))
                        paramY(kalimat(2))
                        paramZ(kalimat(3))
                        paramR(kalimat(4))
                        paramJ1(kalimat(5))
                        paramJ2(kalimat(6))
                        paramJ3(kalimat(7))
                        paramJ4(kalimat(8))
                        buff(kalimat(10))
                        If Label15.Text <> "" Then
                            Bfacum = Label15.Text.Split(kress)
                            buff1(Bfacum(1))
                            If Text00.Text = "0" Then
                                switchV(False)
                                BunifuSwitch1.Value = False
                            ElseIf Text00.Text = "1" Then
                                switchV(True)
                                BunifuSwitch1.Value = True
                            End If
                        End If
                    End If
                    If received.Contains("play") Then
                        Dim play() As String
                        play = received.Split(titikdua)
                        If play(1) = "true" Then
                            A_START.Enabled = True
                            A_START.BackColor = System.Drawing.Color.DarkOrange
                            Lplay.ForeColor = System.Drawing.Color.DarkOrange
                            A_PAUSE.Enabled = False
                            A_PAUSE.BackColor = System.Drawing.Color.Transparent
                            Lpause.ForeColor = System.Drawing.Color.DimGray
                            A_STOP.Enabled = False
                            A_STOP.BackColor = System.Drawing.Color.Transparent
                            Lstop.ForeColor = System.Drawing.Color.DimGray

                        ElseIf play(1) = "false" Then
                            A_START.Enabled = False
                            A_START.BackColor = System.Drawing.Color.Transparent
                            Lplay.ForeColor = System.Drawing.Color.DimGray
                            A_PAUSE.Enabled = True
                            A_PAUSE.BackColor = System.Drawing.Color.DarkOrange
                            Lpause.ForeColor = System.Drawing.Color.DarkOrange
                            A_STOP.Enabled = True
                            A_STOP.BackColor = System.Drawing.Color.DarkOrange
                            Lstop.ForeColor = System.Drawing.Color.DarkOrange
                        End If
                    End If
                End If
            End While
        Catch ex As Exception
            MessageBox.Show("Failed to Receive : " + ex.Message, "Attention!!!", MessageBoxButtons.OK)
        End Try
    End Sub
    Sub Connected()
        Dim toReceive(55) As Byte
        Dim received As String = ""
        Try
            While True
                If client_stream.DataAvailable = True Then
                    client_stream.Read(toReceive, 0, toReceive.Length)
                    received = System.Text.Encoding.ASCII.GetString(toReceive, 0, toReceive.Length)
                    If recv_dat.InvokeRequired Then
                        Dim assign As MethodInvoker
                        assign = New MethodInvoker(AddressOf Connected)
                        recv_dat.Invoke(assign)
                    Else
                        recv_dat.Text = received
                    End If
                End If

            End While
        Catch ex As Exception

        End Try
    End Sub

    Function send(ByVal Data As String)
        Dim sendbyte() As Byte
        Try
            sendbyte = System.Text.Encoding.ASCII.GetBytes(Data)
            PC_Client.Client.Send(sendbyte)
            'TX.WriteLine(Data)
            'TX.Flush()
        Catch ex As Exception
            MessageBox.Show("Error : " + ex.Message, "WARNING!!", MessageBoxButtons.OK)
        End Try
        Return True
    End Function
    Function MSG1(ByVal data As String)
        MsgBox(data)
        Return True
    End Function

    Private Sub sd_data_MouseDown(sender As Object, e As MouseEventArgs) Handles sd_data.MouseDown
        send(send_dat.Text)
    End Sub

    '-----------------PANEL------------------------'
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        recv_dat.Visible = True
        send_dat.Visible = False
        sd_data.Visible = False
        Panel3.Visible = True
        Panel4.Visible = False
        Panel8.Visible = False
        'Bmanual.Enabled = False
        'Bauto.Enabled = False
        'Baboutus.Enabled = False
        Label15.Visible = True
        Text00.Visible = True
        TextBox3.Visible = False
        Bconnect.BackColor = System.Drawing.Color.Gold
        Call aturDGV()
    End Sub

    Private Sub Bconnect_Click(sender As Object, e As EventArgs) Handles Bconnect.Click
        recv_dat.Visible = True
        'send_dat.Visible = False
        'sd_data.Visible = False
        Panel3.Visible = True
        Panel4.Visible = False
        Panel8.Visible = False
        Bconnect.BackColor = System.Drawing.Color.Gold
        Bmanual.BackColor = System.Drawing.Color.DarkOrange
        Bauto.BackColor = System.Drawing.Color.DarkOrange
        Baboutus.BackColor = System.Drawing.Color.DarkOrange
    End Sub

    Private Sub Bmanual_Click(sender As Object, e As EventArgs) Handles Bmanual.Click
        BJOY1.Visible = True
        BJOY2.Visible = False
        Label18.ForeColor = System.Drawing.Color.Green
        Label6.Visible = False
        TextNAMA.Visible = False
        Label41.Text = ""
        Label40.Text = "MANUAL MODE"
        Label3.Text = "START TEACHING"
        Bconnect.BackColor = System.Drawing.Color.DarkOrange
        Bmanual.BackColor = System.Drawing.Color.Gold
        Bauto.BackColor = System.Drawing.Color.DarkOrange
        Baboutus.BackColor = System.Drawing.Color.DarkOrange
        Panel3.Visible = False
        Panel4.Visible = True
        Panel8.Visible = False
        B_TEACHING.Enabled = False
        B_TEACHING.BackColor = System.Drawing.Color.Transparent
        Label3.ForeColor = System.Drawing.Color.DimGray
        B_REMOVE.Enabled = False
        B_REMOVE.BackColor = System.Drawing.Color.Transparent
        Lremove.ForeColor = System.Drawing.Color.DimGray
        T_CANCEL.Enabled = False
        T_CANCEL.BackColor = System.Drawing.Color.Transparent
        Lcancel.ForeColor = System.Drawing.Color.DimGray
        T_RECORD.Enabled = False
        T_RECORD.BackColor = System.Drawing.Color.Transparent
        Lrecord.ForeColor = System.Drawing.Color.DimGray
        T_SAVE.Enabled = False
        T_SAVE.BackColor = System.Drawing.Color.Transparent
        Lsave.ForeColor = System.Drawing.Color.DimGray
        A_CANCEL.Enabled = False
        A_CANCEL.BackColor = System.Drawing.Color.Transparent
        Lcancela.ForeColor = System.Drawing.Color.DimGray
        A_PAUSE.Enabled = False
        A_PAUSE.BackColor = System.Drawing.Color.Transparent
        Lpause.ForeColor = System.Drawing.Color.DimGray
        A_START.Enabled = False
        A_START.BackColor = System.Drawing.Color.Transparent
        Lplay.ForeColor = System.Drawing.Color.DimGray
        A_STOP.Enabled = False
        A_STOP.BackColor = System.Drawing.Color.Transparent
        Lstop.ForeColor = System.Drawing.Color.DimGray

    End Sub

    Private Sub Bauto_Click(sender As Object, e As EventArgs) Handles Bauto.Click

        NumericUpDown1.Value = 1
        NumericUpDown2.Value = 30

        Label40.Text = "TEACHING MODE"
        Label41.Text = "OFF"
        Label17.ForeColor = System.Drawing.Color.DimGray
        Label16.ForeColor = System.Drawing.Color.DimGray
        Label6.Visible = False
        TextNAMA.Visible = False
        Panel3.Visible = False
        Panel4.Visible = True
        Panel8.Visible = False
        send("SPEEDA 30")
        Bconnect.BackColor = System.Drawing.Color.DarkOrange
        Bmanual.BackColor = System.Drawing.Color.DarkOrange
        Bauto.BackColor = System.Drawing.Color.Gold
        Baboutus.BackColor = System.Drawing.Color.DarkOrange
        B_TEACHING.Enabled = True
        B_TEACHING.BackColor = System.Drawing.Color.DarkOrange
        Label3.ForeColor = System.Drawing.Color.DarkOrange
        A_START.Enabled = False
        A_START.BackColor = System.Drawing.Color.Transparent
        Lplay.ForeColor = System.Drawing.Color.DimGray
        A_PAUSE.Enabled = False
        A_PAUSE.BackColor = System.Drawing.Color.Transparent
        Lpause.ForeColor = System.Drawing.Color.DimGray
        A_STOP.Enabled = False
        A_STOP.BackColor = System.Drawing.Color.Transparent
        Lstop.ForeColor = System.Drawing.Color.DimGray
        A_CANCEL.Enabled = False
        A_CANCEL.BackColor = System.Drawing.Color.Transparent
        Lcancela.ForeColor = System.Drawing.Color.DimGray
    End Sub

    Private Sub Baboutus_Click(sender As Object, e As EventArgs) Handles Baboutus.Click
        Bconnect.BackColor = System.Drawing.Color.DarkOrange
        Bmanual.BackColor = System.Drawing.Color.DarkOrange
        Bauto.BackColor = System.Drawing.Color.DarkOrange
        Baboutus.BackColor = System.Drawing.Color.Gold
        Panel3.Visible = False
        Panel4.Visible = False
        Panel8.Visible = True
    End Sub

    '------------------------MANUAL-----------------------'

    Private Sub J1_PLUS_MouseDown(sender As Object, e As MouseEventArgs) Handles J1_PLUS.MouseDown
        send("J1P1")
    End Sub

    Private Sub J1_PLUS_MouseUp(sender As Object, e As MouseEventArgs) Handles J1_PLUS.MouseUp
        send("J1P0")
    End Sub

    Private Sub J1_MIN_MouseDown(sender As Object, e As MouseEventArgs) Handles J1_MIN.MouseDown
        send("J1M1")
    End Sub

    Private Sub J1_MIN_MouseUp(sender As Object, e As MouseEventArgs) Handles J1_MIN.MouseUp
        send("J1M0")
    End Sub

    Private Sub J2_PLUS_MouseDown(sender As Object, e As MouseEventArgs) Handles J2_PLUS.MouseDown
        send("J2P1")
    End Sub

    Private Sub J2_PLUS_MouseUp(sender As Object, e As MouseEventArgs) Handles J2_PLUS.MouseUp
        send("J2P0")
    End Sub

    Private Sub J2_MIN_MouseDown(sender As Object, e As MouseEventArgs) Handles J2_MIN.MouseDown
        send("J2M1")
    End Sub

    Private Sub J2_MIN_MouseUp(sender As Object, e As MouseEventArgs) Handles J2_MIN.MouseUp
        send("J2M0")
    End Sub

    Private Sub J3_PLUS_MouseDown(sender As Object, e As MouseEventArgs) Handles J3_PLUS.MouseDown
        send("J3P1")
    End Sub

    Private Sub J3_PLUS_MouseUp(sender As Object, e As MouseEventArgs) Handles J3_PLUS.MouseUp
        send("J3P0")
    End Sub

    Private Sub J3_MIN_MouseDown(sender As Object, e As MouseEventArgs) Handles J3_MIN.MouseDown
        send("J3M1")
    End Sub

    Private Sub J3_MIN_MouseUp(sender As Object, e As MouseEventArgs) Handles J3_MIN.MouseUp
        send("J3M0")
    End Sub

    Private Sub J4_PLUS_MouseDown(sender As Object, e As MouseEventArgs) Handles J4_PLUS.MouseDown
        send("J4P1")
    End Sub

    Private Sub J4_PLUS_MouseUp(sender As Object, e As MouseEventArgs) Handles J4_PLUS.MouseUp
        send("J4P0")
    End Sub

    Private Sub J4_MIN_MouseDown(sender As Object, e As MouseEventArgs) Handles J4_MIN.MouseDown
        send("J4M1")
    End Sub

    Private Sub J4_MIN_MouseUp(sender As Object, e As MouseEventArgs) Handles J4_MIN.MouseUp
        send("J4M0")
    End Sub


    '-------------------------TEACHING---------------------------'

    Private Sub T_DELETE_Click(sender As Object, e As EventArgs)
        Dim result = MessageBox.Show("YAKIN DATA AKAN DIHAPUS?", "WARNING!!", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
        If result = DialogResult.No Then
            MessageBox.Show("DATA TIDAK JADI DIHAPUS", "", MessageBoxButtons.OK, MessageBoxIcon.Information)
        ElseIf result = DialogResult.Yes Then
            send("T_DELETE")
        End If
    End Sub

    Private Sub T_CANCEL_Click(sender As Object, e As EventArgs) Handles T_CANCEL.Click
        Dim result2 = MessageBox.Show("JIKA BATAL DATA AKAN TERHAPUS", "WARNING!!", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
        If result2 = DialogResult.No Then
            MessageBox.Show("DATA TIDAK JADI DIHAPUS", "", MessageBoxButtons.OK, MessageBoxIcon.Information)
        ElseIf result2 = DialogResult.Yes Then
            Me.DATA_TEACHING.Rows.Clear()
            send("T_BATAL")
            MessageBox.Show("DATA TELAH DIHAPUS", "", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Label3.Text = "START TEACHING"
            Label41.Text = "OFF"
            T_SAVE.Enabled = False
            T_SAVE.BackColor = System.Drawing.Color.Transparent
            Lsave.BackColor = System.Drawing.Color.Transparent
            Lsave.ForeColor = System.Drawing.Color.DimGray
            B_REMOVE.Enabled = False
            B_REMOVE.BackColor = System.Drawing.Color.Transparent
            Lremove.BackColor = System.Drawing.Color.Transparent
            Lremove.ForeColor = System.Drawing.Color.DimGray
            T_CANCEL.Enabled = False
            T_CANCEL.BackColor = System.Drawing.Color.Transparent
            Lcancel.BackColor = System.Drawing.Color.Transparent
            Lcancel.ForeColor = System.Drawing.Color.DimGray
            T_RECORD.Enabled = False
            T_RECORD.BackColor = System.Drawing.Color.Transparent
            Lrecord.BackColor = System.Drawing.Color.Transparent
            Lrecord.ForeColor = System.Drawing.Color.DimGray
            Bconnect.Enabled = True
            Bmanual.Enabled = True
            Bauto.Enabled = True
            Baboutus.Enabled = True
        End If

    End Sub
    Private Sub T_RECORD_Click(sender As Object, e As EventArgs) Handles T_RECORD.Click
        send("T_RECORD ")
        Me.DATA_TEACHING.Rows.Add(TextNAMA.Text, TextX.Text, TextY.Text, TextZ.Text, TextR.Text, TextJ1.Text, TextJ2.Text, TextJ3.Text, TextJ4.Text, TextSuction.Text)

    End Sub

    Private Sub B_REMOVE_Click(sender As Object, e As EventArgs) Handles B_REMOVE.Click
        If DATA_TEACHING.SelectedRows.Count > 0 Then
            For i As Integer = DATA_TEACHING.SelectedRows.Count - 1 To 0 Step -1
                DATA_TEACHING.Rows.RemoveAt(DATA_TEACHING.SelectedRows(i).Index)
                If TextBox3.Text < 10 Then
                    send("REMOVE  " + TextBox3.Text)
                Else
                    send("REMOVE " + TextBox3.Text)
                End If
            Next
            MessageBox.Show("Data Telah Di Hapus", "Perhatian", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub

    Private Sub T_SAVE_Click(sender As Object, e As EventArgs) Handles T_SAVE.Click
        send("SAVE")
        Label3.Text = "START TEACHING"
        Label3.ForeColor = System.Drawing.Color.DimGray
        Label41.Text = "OFF"
        Label41.ForeColor = System.Drawing.Color.DimGray
        Label40.ForeColor = System.Drawing.Color.DimGray
        T_SAVE.Enabled = False
        T_SAVE.BackColor = System.Drawing.Color.Transparent
        Lsave.BackColor = System.Drawing.Color.Transparent
        Lsave.ForeColor = System.Drawing.Color.DimGray
        B_REMOVE.Enabled = False
        B_REMOVE.BackColor = System.Drawing.Color.Transparent
        Lremove.BackColor = System.Drawing.Color.Transparent
        Lremove.ForeColor = System.Drawing.Color.DimGray
        T_CANCEL.Enabled = False
        T_CANCEL.BackColor = System.Drawing.Color.Transparent
        Lcancel.BackColor = System.Drawing.Color.Transparent
        Lcancel.ForeColor = System.Drawing.Color.DimGray
        T_RECORD.Enabled = False
        T_RECORD.BackColor = System.Drawing.Color.Transparent
        Lrecord.BackColor = System.Drawing.Color.Transparent
        Lrecord.ForeColor = System.Drawing.Color.DimGray
        B_TEACHING.Enabled = False
        B_TEACHING.BackColor = System.Drawing.Color.Transparent

        A_CANCEL.Enabled = False
        A_CANCEL.BackColor = System.Drawing.Color.Transparent
        Lcancela.ForeColor = System.Drawing.Color.DimGray
        A_PAUSE.Enabled = False
        A_PAUSE.BackColor = System.Drawing.Color.Transparent
        Lpause.ForeColor = System.Drawing.Color.DimGray
        A_START.Enabled = True
        A_START.BackColor = System.Drawing.Color.DarkOrange
        Lplay.ForeColor = System.Drawing.Color.DarkOrange
        A_STOP.Enabled = False
        A_STOP.BackColor = System.Drawing.Color.Transparent
        Lstop.ForeColor = System.Drawing.Color.DimGray

        Label17.ForeColor = System.Drawing.Color.DarkOrange
        Label16.ForeColor = System.Drawing.Color.DarkOrange
        Label16.Text = "ON"
    End Sub

    '--------------------------------------AUTO--------------------------------------'


    Private Sub A_CANCEL_Click(sender As Object, e As EventArgs) Handles A_CANCEL.Click
        Dim result3 = MessageBox.Show("YAKIN CANCEL? JIKA YA DATA AKAN DIHAPUS", "WARNING!!", MessageBoxButtons.YesNo, MessageBoxIcon.Question)

        If result3 = DialogResult.Yes Then
            MessageBox.Show("DATA DICANCEL", "", MessageBoxButtons.OK, MessageBoxIcon.Information)
            send("T_BATAL")
            B_TEACHING.Enabled = True
            B_TEACHING.BackColor = System.Drawing.Color.DarkOrange
            Label3.Text = "START TEACHING"
            Label41.Text = "OFF"
            T_SAVE.Enabled = False
            T_SAVE.BackColor = System.Drawing.Color.Transparent
            Lsave.BackColor = System.Drawing.Color.Transparent
            Lsave.ForeColor = System.Drawing.Color.DimGray
            B_REMOVE.Enabled = False
            B_REMOVE.BackColor = System.Drawing.Color.Transparent
            Lremove.BackColor = System.Drawing.Color.Transparent
            Lremove.ForeColor = System.Drawing.Color.DimGray
            T_CANCEL.Enabled = False
            T_CANCEL.BackColor = System.Drawing.Color.Transparent
            Lcancel.BackColor = System.Drawing.Color.Transparent
            Lcancel.ForeColor = System.Drawing.Color.DimGray
            T_RECORD.Enabled = False
            T_RECORD.BackColor = System.Drawing.Color.Transparent
            Lrecord.BackColor = System.Drawing.Color.Transparent
            Lrecord.ForeColor = System.Drawing.Color.DimGray
            Label3.ForeColor = System.Drawing.Color.DarkOrange
            Label40.ForeColor = System.Drawing.Color.DarkOrange
            Label41.ForeColor = System.Drawing.Color.DarkOrange
            A_CANCEL.Enabled = False
            A_CANCEL.BackColor = System.Drawing.Color.Transparent
            Lcancela.ForeColor = System.Drawing.Color.DimGray
            A_PAUSE.Enabled = False
            A_PAUSE.BackColor = System.Drawing.Color.Transparent
            Lpause.ForeColor = System.Drawing.Color.DimGray
            A_START.Enabled = False
            A_START.BackColor = System.Drawing.Color.Transparent
            Lplay.ForeColor = System.Drawing.Color.DimGray
            A_STOP.Enabled = False
            A_STOP.BackColor = System.Drawing.Color.Transparent
            Lstop.ForeColor = System.Drawing.Color.DimGray
            Bconnect.Enabled = True
            Bmanual.Enabled = True
            Bauto.Enabled = True
            Baboutus.Enabled = True
            J2_MIN.Enabled = True
            J2_PLUS.Enabled = True
            J1_MIN.Enabled = True
            J1_PLUS.Enabled = True
            J3_MIN.Enabled = True
            J3_PLUS.Enabled = True
            J4_MIN.Enabled = True
            J4_PLUS.Enabled = True
            Label17.ForeColor = System.Drawing.Color.DimGray
            Label16.ForeColor = System.Drawing.Color.DimGray
            Label16.Text = "OFF"
            Me.DATA_TEACHING.Rows.Clear()
        End If
    End Sub


    Private Sub A_PAUSE_Click(sender As Object, e As EventArgs) Handles A_PAUSE.Click
        send("A_PAUSE")
        A_START.Enabled = True
        A_START.BackColor = System.Drawing.Color.DarkOrange
        Lplay.ForeColor = System.Drawing.Color.DarkOrange
    End Sub

    Private Sub A_START_Click(sender As Object, e As EventArgs) Handles A_START.Click
        send("STR " + String.Format(NumericUpDown1.Value) + " " + String.Format(NumericUpDown2.Value))
        A_CANCEL.Enabled = False
        A_CANCEL.BackColor = System.Drawing.Color.Transparent
        Lcancela.ForeColor = System.Drawing.Color.DimGray
        A_STOP.Enabled = True
        A_STOP.BackColor = System.Drawing.Color.DarkOrange
        Lstop.ForeColor = System.Drawing.Color.DarkOrange
        A_PAUSE.Enabled = True
        A_PAUSE.BackColor = System.Drawing.Color.DarkOrange
        Lpause.ForeColor = System.Drawing.Color.DarkOrange
        'A_START.Enabled = False
        'A_START.BackColor = System.Drawing.Color.Transparent
        'Lplay.ForeColor = System.Drawing.Color.DimGray

        Baboutus.Enabled = False
        Bmanual.Enabled = False
        Bconnect.Enabled = False
        Bauto.Enabled = False

        B_TEACHING.Enabled = False
        B_TEACHING.BackColor = System.Drawing.Color.Transparent
        Label3.ForeColor = System.Drawing.Color.DimGray
        Label40.ForeColor = System.Drawing.Color.DimGray
        Label41.ForeColor = System.Drawing.Color.DimGray
        J2_MIN.Enabled = False
        J2_PLUS.Enabled = False
        J1_MIN.Enabled = False
        J1_PLUS.Enabled = False
        J3_MIN.Enabled = False
        J3_PLUS.Enabled = False
        J4_MIN.Enabled = False
        J4_PLUS.Enabled = False
    End Sub

    Private Sub A_STOP_Click(sender As Object, e As EventArgs) Handles A_STOP.Click
        send("A_STOP")
        A_CANCEL.Enabled = True
        A_CANCEL.BackColor = System.Drawing.Color.DarkOrange
        Lcancela.ForeColor = System.Drawing.Color.DarkOrange
        A_START.Enabled = False
        A_START.BackColor = System.Drawing.Color.Transparent
        Lplay.ForeColor = System.Drawing.Color.DimGray
        A_PAUSE.Enabled = False
        A_PAUSE.BackColor = System.Drawing.Color.Transparent
        Lpause.ForeColor = System.Drawing.Color.DimGray
    End Sub

    '--------------------------Data Grid----------------------------------
    Sub aturDGV()
        DATA_TEACHING.Columns("NAMA").Width = 80
        DATA_TEACHING.Columns("X").Width = 50
        DATA_TEACHING.Columns("Y").Width = 50
        DATA_TEACHING.Columns("Z").Width = 50
        DATA_TEACHING.Columns("R").Width = 50
        DATA_TEACHING.Columns("J1").Width = 50
        DATA_TEACHING.Columns("J2").Width = 50
        DATA_TEACHING.Columns("J3").Width = 50
        DATA_TEACHING.Columns("J4").Width = 50
        DATA_TEACHING.Columns("SUCTION").Width = 65

        DATA_TEACHING.RowsDefaultCellStyle.BackColor = Color.LightBlue
        DATA_TEACHING.AlternatingRowsDefaultCellStyle.BackColor = Color.WhiteSmoke
    End Sub

    Private Sub B_TEACHING_Click(sender As Object, e As EventArgs) Handles B_TEACHING.Click
        If Label3.Text = "START TEACHING" Then
            Label3.Text = "STOP TEACHING"
            send("TEACH")
            T_SAVE.Enabled = True
            T_SAVE.BackColor = System.Drawing.Color.DarkOrange
            B_REMOVE.Enabled = True
            B_REMOVE.BackColor = System.Drawing.Color.DarkOrange
            Lremove.ForeColor = System.Drawing.Color.DarkOrange
            T_CANCEL.Enabled = True
            T_CANCEL.BackColor = System.Drawing.Color.DarkOrange
            Lcancel.ForeColor = System.Drawing.Color.DarkOrange
            T_RECORD.Enabled = True
            T_RECORD.BackColor = System.Drawing.Color.DarkOrange
            Lrecord.ForeColor = System.Drawing.Color.DarkOrange
            Lsave.ForeColor = System.Drawing.Color.DarkOrange
            Label41.Text = "ON"
            Label41.ForeColor = System.Drawing.Color.DarkOrange
            Label40.ForeColor = System.Drawing.Color.DarkOrange
            Bconnect.Enabled = False
            Bmanual.Enabled = False
            Bauto.Enabled = False
            Baboutus.Enabled = False
        ElseIf Label3.Text = "STOP TEACHING" Then
            Label3.Text = "START TEACHING"
            Label41.Text = "OFF"
            Label41.ForeColor = System.Drawing.Color.DimGray
            Label40.ForeColor = System.Drawing.Color.DimGray
            T_SAVE.Enabled = False
            T_SAVE.BackColor = System.Drawing.Color.Transparent
            Lsave.BackColor = System.Drawing.Color.Transparent
            Lsave.ForeColor = System.Drawing.Color.DimGray
            B_REMOVE.Enabled = False
            B_REMOVE.BackColor = System.Drawing.Color.Transparent
            Lremove.BackColor = System.Drawing.Color.Transparent
            Lremove.ForeColor = System.Drawing.Color.DimGray
            T_CANCEL.Enabled = False
            T_CANCEL.BackColor = System.Drawing.Color.Transparent
            Lcancel.BackColor = System.Drawing.Color.Transparent
            Lcancel.ForeColor = System.Drawing.Color.DimGray
            T_RECORD.Enabled = False
            T_RECORD.BackColor = System.Drawing.Color.Transparent
            Lrecord.BackColor = System.Drawing.Color.Transparent
            Lrecord.ForeColor = System.Drawing.Color.DimGray
            Bconnect.Enabled = True
            Bmanual.Enabled = True
            Bauto.Enabled = True
            Baboutus.Enabled = True

        End If

    End Sub

    Private Sub TrackBar1_MouseUp(sender As Object, e As MouseEventArgs) Handles TrackBar1.MouseUp
        If TrackBar1.Value < 10 Then
            Label38.Text = TrackBar1.Value
            TextBox1.Text = "SPEEDM   " + String.Format(Label38.Text)
            send(TextBox1.Text)
            TextBox1.Clear()
        ElseIf TrackBar1.Value > 10 And TrackBar1.Value <= 99 Then
            Label38.Text = TrackBar1.Value
            TextBox1.Text = "SPEEDM  " + String.Format(Label38.Text)
            send(TextBox1.Text)
            TextBox1.Clear()
        ElseIf TrackBar1.Value > 99 Then
            Label38.Text = TrackBar1.Value
            TextBox1.Text = "SPEEDM " + String.Format(Label38.Text)
            send(TextBox1.Text)
            TextBox1.Clear()
        End If
    End Sub


    Private Sub NumericUpDown2_ValueChanged(sender As Object, e As EventArgs) Handles NumericUpDown2.ValueChanged
        'If Label16.Text = "ON" Then
        'send("SPEEDA " + String.Format(NumericUpDown2.Value))
        'End If
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        send("EMG")
    End Sub
    Private Sub Form1_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        send("EXIT")
    End Sub
    Private Sub DATA_TEACHING_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles DATA_TEACHING.CellClick
        TextBox3.Text = DATA_TEACHING.CurrentRow.Index.ToString
    End Sub

    Private Sub BunifuSwitch1_Click(sender As Object, e As EventArgs) Handles BunifuSwitch1.Click
        If BunifuSwitch1.Value = False Then
            send("Vof")
            TextSuction.Text = "OFF"
            Button9.BackColor = System.Drawing.Color.Gray
        ElseIf BunifuSwitch1.Value = True Then
            send("Von")
            TextSuction.Text = "ON"
            Button9.BackColor = System.Drawing.Color.Red
        End If
    End Sub

    Private Sub BJOY2_Click(sender As Object, e As EventArgs) Handles BJOY2.Click
        BJOY1.Visible = True
        BJOY2.Visible = False
        Label18.ForeColor = System.Drawing.Color.Green
        send("JOYOF")
    End Sub

    Private Sub BJOY1_Click(sender As Object, e As EventArgs) Handles BJOY1.Click
        BJOY1.Visible = False
        BJOY2.Visible = True
        Label18.ForeColor = System.Drawing.Color.DarkOrange
        send("JOYON")
    End Sub
End Class

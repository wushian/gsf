'*******************************************************************************************************
'  Tva.Services.ClientHelper.vb - Client Request to Service
'  Copyright � 2006 - TVA, all rights reserved - Gbtc
'
'  Build Environment: VB.NET, Visual Studio 2005
'  Primary Developer: Pinal C. Patel, Operations Data Architecture [TVA]
'      Office: COO - TRNS/PWR ELEC SYS O, CHATTANOOGA, TN - MR 2W-C
'       Phone: 423/751-2250
'       Email: pcpatel@tva.gov
'
'  Code Modification History:
'  -----------------------------------------------------------------------------------------------------
'  08/29/2006 - Pinal C. Patel
'       Original version of source code generated
'
'*******************************************************************************************************

Imports System.Text
Imports System.Drawing
Imports System.ComponentModel
Imports Tva.Communication
Imports Tva.Serialization

<ToolboxBitmap(GetType(ClientHelper))> _
Public Class ClientHelper

    ''' <summary>
    ''' Occurs when a response is received from the service.
    ''' </summary>
    ''' <param name="response">The response received from the service.</param>
    Public Event ReceivedServiceResponse(ByVal response As ServiceResponse)

    ''' <summary>
    ''' Occurs when the service client needs to update its status.
    ''' </summary>
    ''' <param name="message">The message that the service client must display in its status.</param>
    Public Event UpdateClientStatus(ByVal message As String)

    Public Event ServiceStateChanged(ByVal serviceName As String, ByVal serviceState As ServiceState)

    Public Event ProcessStateChanged(ByVal processName As String, ByVal processState As ProcessState)

    ''' <summary>
    ''' Gets the instance of TCP client used for communicating with the service.
    ''' </summary>
    ''' <value></value>
    ''' <returns>An instance of TCP client.</returns>
    <TypeConverter(GetType(ExpandableObjectConverter)), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)> _
    Public ReadOnly Property TcpClient() As TcpClient
        Get
            Return CHTcpClient
        End Get
    End Property

    ''' <summary>
    ''' Attempts to connect to the service.
    ''' </summary>
    ''' <remarks>This method must be called in order to establish connection with the service.</remarks>
    Public Sub Connect()

        CHTcpClient.Connect()

    End Sub

    Public Sub Disconnect()

        CHTcpClient.Disconnect()

    End Sub

    ''' <summary>
    ''' Sends a request to the service.
    ''' </summary>
    ''' <param name="request">The request to be sent to the service.</param>
    Public Sub SendRequest(ByVal request As ClientRequest)

        CHTcpClient.Send(request)

    End Sub

#Region " TcpClient Events "

    Private Sub CHTcpClient_Connected(ByVal sender As Object, ByVal e As System.EventArgs) Handles CHTcpClient.Connected

        CHTcpClient.Send(New ClientInfo())
        UpdateStatus("Successfully connected to the service!", True, 0, 2)

    End Sub

    Private Sub CHTcpClient_Connecting(ByVal sender As Object, ByVal e As System.EventArgs) Handles CHTcpClient.Connecting

        UpdateStatus("Attempting to connect to the service...", True, 0, 2)

    End Sub

    Private Sub CHTcpClient_ConnectingException(ByVal ex As System.Exception) Handles CHTcpClient.ConnectingException

        With New StringBuilder()
            .Append("Failed to connect to the service due to an exception:")
            .Append(Environment.NewLine)
            .Append(ex.Message)

            UpdateStatus(.ToString(), True, 0, 2)
        End With

    End Sub

    Private Sub CHTcpClient_Disconnected(ByVal sender As Object, ByVal e As System.EventArgs) Handles CHTcpClient.Disconnected

        UpdateStatus("Disconnected from the service.", True, 0, 2)

    End Sub

    Private Sub CHTcpClient_ReceivedData(ByVal data() As System.Byte) Handles CHTcpClient.ReceivedData

        Dim response As ServiceResponse = GetObject(Of ServiceResponse)(data)
        If response IsNot Nothing Then
            RaiseEvent ReceivedServiceResponse(response)
            Select Case response.Type
                Case "UPDATECLIENTSTATUS"
                    UpdateStatus(response.Message, True, 1, 1)
                Case "SERVICESTATECHANGED"
                    Dim messageSegments As String() = response.Message.Split(">"c)
                    If messageSegments.Length = 2 Then
                        With New StringBuilder()
                            .Append("Received ServiceStateChanged service response.")
                            .Append(Environment.NewLine)
                            .Append("              Service Name: ")
                            .Append(messageSegments(0))
                            .Append(Environment.NewLine)
                            .Append("             Service State: ")
                            .Append(messageSegments(1))

                            UpdateStatus(.ToString(), True, 1, 2)
                        End With
                        RaiseEvent ServiceStateChanged(messageSegments(0), DirectCast(System.Enum.Parse(GetType(ServiceState), messageSegments(1)), ServiceState))
                    End If
                Case "PROCESSSTATECHANGED"
                    Dim messageSegments As String() = response.Message.Split(">"c)
                    If messageSegments.Length = 2 Then
                        With New StringBuilder()
                            .Append("Received ProcessStateChanged service response.")
                            .Append(Environment.NewLine)
                            .Append("              Process Name: ")
                            .Append(messageSegments(0))
                            .Append(Environment.NewLine)
                            .Append("             Process State: ")
                            .Append(messageSegments(1))

                            UpdateStatus(.ToString(), True, 1, 2)
                        End With
                        RaiseEvent ProcessStateChanged(messageSegments(0), DirectCast(System.Enum.Parse(GetType(ProcessState), messageSegments(1)), ProcessState))
                    End If
            End Select
        End If

    End Sub

#End Region

#Region " Private Methods "

    Private Sub UpdateStatus(ByVal message As String, ByVal showDateTime As Boolean, _
            ByVal prependedCrlf As Integer, ByVal appendedCrlf As Integer)

        With New StringBuilder()
            For i As Integer = 0 To prependedCrlf - 1
                .Append(Environment.NewLine)
            Next
            If showDateTime Then
                .Append("[")
                .Append(System.DateTime.Now.ToString())
                .Append("] ")
                .Append(Environment.NewLine)
            End If
            .Append(message)
            For i As Integer = 0 To appendedCrlf - 1
                .Append(Environment.NewLine)
            Next

            RaiseEvent UpdateClientStatus(.ToString())
        End With

    End Sub

#End Region

End Class

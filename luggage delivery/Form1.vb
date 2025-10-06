Public Class Form1
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click


        ListBox1.TopIndex = ListBox1.Items.Count - 1

        ' Toggle button color
        Button1.BackColor = If(Button1.BackColor = Color.GreenYellow, Color.SteelBlue, Color.GreenYellow)

        ' Clear previous output
        ListBox1.Items.Clear()

        ' Constants
        Const LUGGAGE_CAPACITY As Decimal = 50000000D ' ₱50,000,000 per luggage
        Const VAN_COUNT As Integer = 6
        Const DEFAULT_AMOUNT As Decimal = 1000000000D

        ' Get amount from TextBox or use default
        Dim totalAmount As Decimal
        Dim usedDefault As Boolean = False

        If Not Decimal.TryParse(TextBox1.Text, totalAmount) OrElse totalAmount <= 0 Then
            totalAmount = DEFAULT_AMOUNT
            usedDefault = True
        End If

        ' Show delivery destination
        ListBox1.Items.Add("📦 Luggage Delivery to Shangri-La BGC")

        ' Show amount entered with words
        ListBox1.Items.Add("Amount of Money Entered: ₱" & Format(totalAmount, "#,##0.00") & " (" & ConvertAmountToWords(totalAmount) & " pesos)")

        ' Show default notice if used
        If usedDefault Then
            ListBox1.Items.Add("💡 Default amount of ₱1,000,000,000 was used due to missing or invalid input.")
        End If

        ' Notify based on amount range
        If totalAmount > DEFAULT_AMOUNT Then
            ListBox1.Items.Add("⚠ Note: This amount exceeds ₱1,000,000,000 and is considered user-provided, not from the company.")
        ElseIf totalAmount < DEFAULT_AMOUNT Then
            ListBox1.Items.Add("ℹ Note: This amount is below ₱1,000,000,000 and may be considered partial or user-provided.")
        Else
            ListBox1.Items.Add("✅ Exact company amount of ₱1,000,000,000 was used.")
        End If

        ' Calculate total luggage needed
        Dim totalLuggage As Decimal = Math.Ceiling(totalAmount / LUGGAGE_CAPACITY)
        ListBox1.Items.Add("Total Luggage Used: " & totalLuggage)
        ListBox1.Items.Add("Luggage per Van:")

        ' Array to store luggage per van
        Dim luggagePerVan(VAN_COUNT - 1) As Decimal
        Dim baseLuggage As Decimal = totalLuggage \ VAN_COUNT
        Dim extraLuggage As Decimal = totalLuggage Mod VAN_COUNT

        ' Distribute luggage across vans
        For vanNumber As Integer = 0 To VAN_COUNT - 1
            luggagePerVan(vanNumber) = baseLuggage
            If vanNumber < extraLuggage Then
                luggagePerVan(vanNumber) += 1
            End If
            ListBox1.Items.Add("Van " & (vanNumber + 1) & ": " & luggagePerVan(vanNumber))
        Next
    End Sub

    ' Converts a number to words (basic version for whole pesos)
    Private Function ConvertAmountToWords(amount As Decimal) As String
        Dim value As Long = Math.Floor(amount)

        Dim words As String = ""
        Try
            Dim culture As New Globalization.CultureInfo("en-US")
            words = Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(New Globalization.CultureInfo("en-US").TextInfo.ToString(value))
        Catch
            words = value.ToString()
        End Try
        Return NumberToWords(value)
    End Function

    ' Basic number-to-words converter (supports up to billions)
    Private Function NumberToWords(number As Long) As String
        If number = 0 Then Return "zero"
        If number < 0 Then Return "minus " & NumberToWords(Math.Abs(number))

        Dim words As String = ""

        If (number \ 1000000000) > 0 Then
            words += NumberToWords(number \ 1000000000) & " billion "
            number = number Mod 1000000000
        End If
        If (number \ 1000000) > 0 Then
            words += NumberToWords(number \ 1000000) & " million "
            number = number Mod 1000000
        End If
        If (number \ 1000) > 0 Then
            words += NumberToWords(number \ 1000) & " thousand "
            number = number Mod 1000
        End If
        If (number \ 100) > 0 Then
            words += NumberToWords(number \ 100) & " hundred "
            number = number Mod 100
        End If
        If number > 0 Then
            If words <> "" Then words += "and "
            Dim unitsMap As String() = {"zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten",
                                    "eleven", "twelve", "thirteen", "fourteen", "fifteen", "sixteen",
                                    "seventeen", "eighteen", "nineteen"}
            Dim tensMap As String() = {"zero", "ten", "twenty", "thirty", "forty", "fifty", "sixty", "seventy", "eighty", "ninety"}

            If number < 20 Then
                words += unitsMap(number)
            Else
                words += tensMap(number \ 10)
                If (number Mod 10) > 0 Then
                    words += "-" & unitsMap(number Mod 10)
                End If
            End If
        End If

        Return words.Trim()

        ListBox1.TopIndex = ListBox1.Items.Count - 1

    End Function

    Private Sub TextBox1_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextBox1.KeyPress
        ' Allow only digits and control keys (like Backspace)
        If Not Char.IsControl(e.KeyChar) AndAlso Not Char.IsDigit(e.KeyChar) Then
            e.Handled = True ' Block the key
        End If
    End Sub
End Class

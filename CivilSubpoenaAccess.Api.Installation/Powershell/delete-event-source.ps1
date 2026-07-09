param
(
    [Parameter(Mandatory = $true)]
    [string]$Source
)

if ([System.Diagnostics.EventLog]::SourceExists($Source))
{
    Remove-EventLog -Source $Source

    Write-Host "Removed event source '$Source'."
}
else
{
    Write-Host "Event source '$Source' does not exist."
}
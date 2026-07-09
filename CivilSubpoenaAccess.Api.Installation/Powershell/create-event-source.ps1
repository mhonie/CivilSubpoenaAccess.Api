param
(
    [Parameter(Mandatory = $true)]
    [string]$Source
)

$logName = "Application"

if (-not [System.Diagnostics.EventLog]::SourceExists($Source))
{
    New-EventLog -LogName $logName -Source $Source

    Write-Host "Created event source '$Source'."
}
else
{
    Write-Host "Event source '$Source' already exists."
}
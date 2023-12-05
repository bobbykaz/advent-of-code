$env:INPUT_ROOT = Get-Location
$env:AOC_TOKEN = Get-Content -Raw "token.txt"
dotnet run
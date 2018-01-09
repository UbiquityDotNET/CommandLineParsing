if( !(Test-Path ".\BuildOutput\docs\.git" -PathType Container))
{
    md -Force BuildOutput -ErrorAction SilentlyContinue
    Write-Information "Cloning Docs repo"
    pushd BuildOutput -ErrorAction Stop
    try
    {
        git clone https://github.com/UbiquityDotNET/CommandlineParsing.git -b gh-pages docs -q
    }
    finally
    {
        popd
    }
}
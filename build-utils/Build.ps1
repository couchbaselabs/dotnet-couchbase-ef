# is this a tagged build?
if ($env:APPVEYOR_REPO_TAG -eq "true") {
    # use tag as version
    $versionNumber = "$env:APPVEYOR_REPO_TAG_NAME"
} else {
    # create pre-release build number based on AppVeyor build number
    $buildCounter = "$env:APPVEYOR_BUILD_NUMBER".PadLeft(6, "0")
    $versionNumber = .\build-utils\AutoVersionNumber.ps1 -VersionSuffix "alpha-$buildCounter"
}

Write-Host "Using version: $versionNumber"
Update-AppveyorBuild -Version $versionNumber

dotnet build -c Release -p:Version=${env:APPVEYOR_BUILD_VERSION} ./Couchbase.EntityFramework.sln
dotnet pack -c Release -p:Version=${env:APPVEYOR_BUILD_VERSION} ./src/Couchbase.EntityFramework/Couchbase.EntityFramework.csproj

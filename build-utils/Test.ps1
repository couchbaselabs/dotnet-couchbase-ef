Param(
    [string]
    $Logger
)

dotnet test -c Release -l $Logger ./test/Couchbase.EntityFramework.UnitTests/Couchbase.EntityFramework.UnitTests.csproj

param(
    [Parameter(Mandatory = $true)]
    [string]$p,

    [Parameter(Mandatory = $true)]
    [string]$fn,

    [Parameter(Mandatory = $true)]
    [string]$pfn,

    [Parameter(Mandatory = $true)]
    [ValidateSet("Query", "Command")]
    [string]$rq,

    [string]$Path = (Get-Location).Path
)

$Path = (Resolve-Path $Path).Path

$targetFolder = Join-Path $Path (Join-Path $p $fn)

if (Test-Path $targetFolder) {
    Write-Host "Folder '$targetFolder' da ton tai. File trung ten se bi ghi de." -ForegroundColor Yellow
} else {
    New-Item -ItemType Directory -Path $targetFolder -Force | Out-Null
}

$normalizedPath = $Path -replace '\\', '/'
$segments = $normalizedPath.Split('/') | Where-Object { $_ -ne '' }

$appIndex = -1
for ($i = 0; $i -lt $segments.Length; $i++) {
    if ($segments[$i] -match 'Application$') {
        $appIndex = $i
        break
    }
}

if ($appIndex -ge 0) {
    $nsSegments = $segments[$appIndex..($segments.Length - 1)]
    $baseNamespace = ($nsSegments -join '.')
} else {
    Write-Host "Khong tim thay segment 'Application' trong duong dan, dung namespace mac dinh." -ForegroundColor Yellow
    $baseNamespace = "ResumeBuilder.Application.Features"
}

$namespace = "$baseNamespace.$p.$fn"

Write-Host "Namespace: $namespace" -ForegroundColor Cyan
Write-Host "Target folder: $targetFolder" -ForegroundColor Cyan
Write-Host "Request type: $rq" -ForegroundColor Cyan

$requestClassName = "${pfn}${rq}"
$handlerClassName = "${requestClassName}Handler"
$validatorClassName = "${requestClassName}Validator"
$responseClassName = "${pfn}Response"

$requestContent = @"
using MediatR;

namespace $namespace;

public sealed record ${requestClassName}() : IRequest<${responseClassName}>;
"@

$handlerContent = @"
using MediatR;

namespace $namespace;

public sealed class ${handlerClassName} : IRequestHandler<${requestClassName}, ${responseClassName}>
{
    public async Task<${responseClassName}> Handle(${requestClassName} request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
"@

$validatorContent = @"
using FluentValidation;

namespace $namespace;

public sealed class ${validatorClassName} : AbstractValidator<${requestClassName}>
{
    public ${validatorClassName}()
    {
    }
}
"@

$responseContent = @"
namespace $namespace;

public sealed record ${responseClassName}();
"@

$utf8NoBom = New-Object System.Text.UTF8Encoding($false)

function Write-FileUtf8NoBom {
    param([string]$FilePath, [string]$Content)
    [System.IO.File]::WriteAllText($FilePath, $Content, $utf8NoBom)
    Write-Host "Created: $FilePath" -ForegroundColor Green
}

Write-FileUtf8NoBom -FilePath (Join-Path $targetFolder "${requestClassName}.cs") -Content $requestContent
Write-FileUtf8NoBom -FilePath (Join-Path $targetFolder "${handlerClassName}.cs") -Content $handlerContent
Write-FileUtf8NoBom -FilePath (Join-Path $targetFolder "${validatorClassName}.cs") -Content $validatorContent
Write-FileUtf8NoBom -FilePath (Join-Path $targetFolder "${responseClassName}.cs") -Content $responseContent

Write-Host ""
Write-Host "Done! 4 file da duoc tao trong: $targetFolder" -ForegroundColor Magenta

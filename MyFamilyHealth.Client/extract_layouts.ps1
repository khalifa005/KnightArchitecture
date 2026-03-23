$files = @(
  @{ Src="StitchDownloads\03_Access_Sharing_Control.html"; Dest="src\app\features\access-control\access-control.html" },
  @{ Src="StitchDownloads\04_AI_Health_Assistant.html"; Dest="src\app\features\ai-assistant\ai-assistant.html" },
  @{ Src="StitchDownloads\05_Medication_Manager.html"; Dest="src\app\features\medication-manager\medication-manager.html" },
  @{ Src="StitchDownloads\06_Medical_Records.html"; Dest="src\app\features\medical-records\medical-records.html" }
)

foreach ($file in $files) {
  $content = Get-Content $file.Src -Raw
  if ($content -match "(?s)<main[^>]*>(.*?)</main>") {
    Set-Content -Path $file.Dest -Value $matches[1].Trim()
  }
}

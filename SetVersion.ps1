# Set the build version in the project.json files

Get-ChildItem . -recurse project.json | 
		foreach {
			$projectJson = $_.FullName
			$jsonContent = Get-Content $projectJson -raw | ConvertFrom-Json
			$version = $jsonContent.version
			echo "Changing Version: $version to $env:APPVEYOR_BUILD_VERSION"
			$jsonContent.version = $env:APPVEYOR_BUILD_VERSION
			$jsonContent | ConvertTo-Json  | set-content $_.FullName
		}

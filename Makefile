fake_run_cmd = dotnet run --project ./build/build.fsproj -t

all:
	$(fake_run_cmd) "All"

buildapp:
	$(fake_run_cmd) "BuildApp"

buildtests:
	$(fake_run_cmd) "BuildTests"

runtests:
	$(fake_run_cmd) "RunTests"

rununit:
	$(fake_run_cmd) "RunTests" --unit-only

clean:
	$(fake_run_cmd) "Clean"

restore:
	$(fake_run_cmd) "Restore"

fmt:
	find ./src/ -type f -name "*.fs" -not -path "*obj*" | xargs dotnet fantomas

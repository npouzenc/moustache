# moustache

_moustache_ helps converting YAML data sources with mustache logicless templates to ... anything.

## Usage

```
USAGE:
Normal scenario:
  moustache --input hash.yaml configuration.yaml --mustache template.mustache
--output result.html --partials partial1.mustache --verbose
Normal scenario:
  moustache --input hash.json --mustache graphviz.mustache --output
mydiagram.dot

  -i, --input       Required. Input hash file(s) to be processed with a
                    mustache semantic template (json or YAML input).

  -m, --mustache    Required. Input Mustache or Handlebars template.

  -p, --partials    Registering partials template for mustache.

  -o, --output      Output file is specified, default is standart output.

  -v, --verbose     Print details during execution.

  --help            Display this help screen.

  --version         Display version information.
```

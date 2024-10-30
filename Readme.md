# Aws Environment Finder

Em muitas situações precisamos encontrar uma forma de definir quais aplicações fazem referência \
a um determinado serviço como uma instância de banco de dados que precisa ser migrada ou algum broker que mudou de endereço por exemplo.

Com esse proposito de identificar essas ocorrências, essa ferramenta ***varre*** todos os containers ECS e encontra dentro das variáveis de ambiente \
uma lista parametrizada de "Keys" que sejam compatíveis.

A saída é um log desses containers ECS que fazem referência a alguma Key encontrada.

## Configuração

Exemplo de configuração, onde a seção AwsCredential precisa ser configurada com sua credencial IAM \
e a seção FindFor com o array de chaves que deseja pesquisar

```appsettings
{
    "AwsCredential": {
        "Region": "sa-east-1",
        "AccessKey": "",
        "SecretKey": ""
    },
    "FindFor": {
        "Keys": [
            "172.18.1.24",
            "bolivia",
            "$("
        ]
    }
}
```

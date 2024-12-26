# ElasticSearchExample
.NET Core MVC ile Elastic Search Kullanımı - Library : Elastic Client


1 - Blog şemasını elastic search üzerinde oluşturalım.

PUT blog
{
  "mappings": {
    "properties": {
      "title":{
        "type": "text",
        "fields": {
          "keyword":{
            "type":"keyword"
          }
        }
      },
      "content":{
        "type": "text"
      },
      "user_id":{
        "type": "keyword"
      },
      "tags":{
        "type": "keyword"
      },
      "created":{
        "type": "date"
      }
    }
  }
}

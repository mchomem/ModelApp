Server = {

    Configuration: {
        schema: 'https'
        , domainOrIp: 'localhost'
        , port: '7141'
    }
    , setService: function (service) {
        var server = Server.Configuration.schema.concat('://')
            .concat(Server.Configuration.domainOrIp).concat(':')
            .concat(Server.Configuration.port).concat('/');
        return server + service;
    }

};
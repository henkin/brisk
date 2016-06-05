'use strict'
var r = require('rethinkdbdash')();

function eventer()
{
    let initialized = false;
    return {
        raise(event) {
            let initPromise = Promise.resolve();
            if (!initialized)
            {
                initPromise = createDatabaseAndTable();
                initialized = true;
            }
            //console.log("event raised! ", event);
            return initPromise.then(function() {
                return r.db("brisk").table('events').insert({ev: event}).run().then(function (x) {
                    console.log("insert happened", x);
                }).error(handleError)
            });
        }
    }

    function handleError(err) {
        console.error(err);
    }

    function createDatabaseAndTable() {
        return r.dbCreate('brisk').run().then(function(x) {
            console.log('db created: ', x)
        }).then(function(z) {
            return r.db('brisk').tableCreate('events').run().then(function(y) {
                console.log('table created', y);
            });
        }).error(handleError);
    }
}

module.exports = eventer();
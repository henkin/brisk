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
                return insertEvent(event);
            });
        }
    }

    function insertEvent(event) {
        return r.db("brisk").table('events').insert(event).run().then(function (x) {
            console.log("event inserted", x);
        }).error(handleError);
    }

    function handleError(err) {
        console.error(err);
    }

    function createDatabaseAndTable() {
        return r.dbList().contains('brisk')
            .do(function (databaseExists) {
                return r.branch(
                    databaseExists,
                    {dbs_created: 0},
                    r.dbCreate('brisk')
                );
            }).run()
            .then(function (x) {
                console.log('db exists: ', x)
            })
            .then(function (z) {
                return r.db('brisk').tableCreate('events').run().then(function (y) {
                    console.log('table created', y);
                }).catch(function(y) {
                    console.log("failed to create table");
                });
            }).error(handleError);
    }
}

module.exports = eventer();
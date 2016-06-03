'use strict';

class ThingCommand {

}

module.exports = class Tasks extends ThingCommand {
    getAll() { return "foo"; }
    create(body) { console.log('creating body: ', body)}
}


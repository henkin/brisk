'use strict'
let eventer = require('../eventer/eventer');

class BaseCommand {
    constructor(item) {
        this._item = item;
    }
    run() {
        //return new Promise(function(resolve, reject) {
            console.log("BaseCommand.run: ", this._item);
            return eventer.raise(this._item).then(function(x) {
                console.log("raised!"); //{success: true})
                return { success: true };
            });
        //})
    }
}

class CreateCommand extends BaseCommand {
    constructor(item) {
        super(item);
    }
}

module.exports = CreateCommand;

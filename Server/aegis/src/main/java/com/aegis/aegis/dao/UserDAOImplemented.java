/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package com.aegis.aegis.dao;

import com.aegis.aegis.modal.Role;
import com.aegis.aegis.modal.User;
import dto.loginDto;
import exception.BadGatewayException;
import exception.RecordNotFoundException;
import java.util.List;
import javax.persistence.EntityManager;
import org.hibernate.Session;
import org.hibernate.query.Query;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Repository;

import static java.nio.charset.StandardCharsets.UTF_8;
import org.jasypt.util.text.AES256TextEncryptor;

@Repository
public class UserDAOImplemented implements UserDAO {

    private final static String PASSWORD = "This is a password";
    @Autowired
    private EntityManager entityManager;

    @Override
    public List<User> get() {
        Session currSession = entityManager.unwrap(Session.class);
        Query<User> query = currSession.createQuery("from User", User.class);
        List<User> list = query.getResultList();
        return list;
    }

    @Override
    public User get(int id) {
        try {
            Session currSession = entityManager.unwrap(Session.class);
            User user = currSession.get(User.class, id);
            AES256TextEncryptor textEncryptor = new AES256TextEncryptor();
            textEncryptor.setPassword(PASSWORD);
            user.setPassword(textEncryptor.decrypt(user.getPassword()));
            return user;
        } catch (Exception ex) {
            throw new RecordNotFoundException("No User record exists for given id", id + "");
        }

    }

    @Override
    public void delete(int id) {
        Session currSession = entityManager.unwrap(Session.class);
        User user = currSession.get(User.class, id);
        currSession.delete(user);
    }

    @Override
    public boolean checkLogin(String username, String password) {
        Session currSession = entityManager.unwrap(Session.class);
        boolean userFound = false;
        //Query using Hibernate Query Language
        //String SQL_QUERY = "from User as o where o.username='"+username+"' and o.password='"+password+"'";
        String SQL_QUERY = "from User as o where o.username=?0 and password=?1";
        Query query = currSession.createQuery(SQL_QUERY);
        query.setParameter(0, username);
        AES256TextEncryptor textEncryptor = new AES256TextEncryptor();
        textEncryptor.setPassword(PASSWORD);
        query.setParameter(1, textEncryptor.encrypt(password));
        List list = query.list();
        if ((list != null) /*&& (list.size() > 0)*/ && (!list.isEmpty())) {
            userFound = true;
        }
        currSession.close();
        return userFound;
    }

    @Override
    public void save(loginDto user) {
        User u;
        try {
            u = this.findByUsername(user.getUsername());
            if (u != null) {
                throw new BadGatewayException("That username is already taken.", user.getUsername());
            }
        } catch (RecordNotFoundException re) {
            if (validate(user)) {
                AES256TextEncryptor textEncryptor = new AES256TextEncryptor();
                textEncryptor.setPassword(PASSWORD);
                u = new User();
                u.setRole_Id(2);
                u.setUsername(user.getUsername());
                u.setPassword(textEncryptor.encrypt(user.getPassword()));
                Session currSession = entityManager.unwrap(Session.class);
                currSession.saveOrUpdate(u);
            } else {
                throw new BadGatewayException("Invalid user fields", "");
            }
        }
    }

    @Override
    public boolean validate(loginDto user) {
        return (user.getUsername().length() >= 6 && user.getUsername().length() < 45) && (user.getPassword().length() >= 8 || user.getPassword().length() < 45);
    }

    @Override
    public User findByUsername(String username) {
        Session currSession = entityManager.unwrap(Session.class);
        String SQL_QUERY = "from User as o where o.username=?0";
        Query query = currSession.createQuery(SQL_QUERY);
        query.setParameter(0, username);
        List list = query.list();
        if ((list != null) && (!list.isEmpty())) {
            User user = (User) list.get(0);
            AES256TextEncryptor textEncryptor = new AES256TextEncryptor();
            textEncryptor.setPassword(PASSWORD);
            user.setPassword(textEncryptor.decrypt(user.getPassword()));
            return user;
            //return (User) list.get(0);
        }
        throw new RecordNotFoundException("No User record exists for given username", username);
    }

}
